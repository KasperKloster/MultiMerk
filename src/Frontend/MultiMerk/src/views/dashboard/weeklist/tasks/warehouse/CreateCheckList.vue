<script setup>
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import api from '@/utils/api';
import Header from '@/components/layout/Header.vue';
import BackToWeeklistLink from '@/components/layout/BackToWeeklistLink.vue';

const route = useRoute();
const weeklistId = route.params.id;
const successMessage = ref('');
const errorMessage = ref('');
const selectedFile = ref(null);


const getXlsFile = async () => {
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';

    // Setting formdata
    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(
            '/weeklist/warehouse/get-checklist',
            formData,
            { responseType: 'blob' }
        );

        // Default filename    
        let fileName = `${weeklistId}-Checklist.xls`;

        // Extract filename from content-disposition header
        const disposition = response.headers['content-disposition'];
        if (disposition) {
            const utf8Match = disposition.match(/filename\*\=UTF-8''(.+?)(?:;|$)/);
            const asciiMatch = disposition.match(/filename="?(.*?)"?(\s*;|$)/);

            if (utf8Match?.[1]) {
                fileName = decodeURIComponent(utf8Match[1]);
            } else if (asciiMatch?.[1]) {
                fileName = asciiMatch[1];
            }
        }

        const blob = new Blob([response.data], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();

        successMessage.value = 'Download successful';
    } catch (error) {
        console.error(error);
        const errorText = await error?.response?.data?.text?.() ?? 'An unknown error occurred.';
        errorMessage.value = `Download failed: ${errorText}`;
    }

};

// Upload
const onFileChange = (event) => {
    const file = event.target.files[0];

    if (file && file.type === 'application/vnd.ms-excel') {
        selectedFile.value = file;
    } else {
        errorMessage.value = 'Please, upload an .xls file..';
        console.error('Please, upload an .xls file')
        selectedFile.value = null;
    }
}

const handleUpload = async () => {
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';
    if (!selectedFile.value) return;

    // Setting formdata
    const formData = new FormData();
    // Append
    formData.append('file', selectedFile.value);
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(`/weeklist/warehouse/upload-checklist`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        successMessage.value = `Success`
        selectedFile.value = null;

    } catch (error) {
        console.log(error);
        errorMessage.value = `Upload failed: ${error.response.data}`;
    }
};

</script>

<template>
    <Header title="Create Checklist " />
    <BackToWeeklistLink />
    <div class="w-full max-w-5xl mx-auto p-6 bg-white rounded-xl shadow-md space-y-4">
        <button @click="getXlsFile()" type="button"
            class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-10 py-2.5 text-center inline-flex items-center me-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 cursor-pointer">
            Download Checklist
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="size-6 ml-2">
                <path fill-rule="evenodd"
                    d="M12 2.25a.75.75 0 0 1 .75.75v11.69l3.22-3.22a.75.75 0 1 1 1.06 1.06l-4.5 4.5a.75.75 0 0 1-1.06 0l-4.5-4.5a.75.75 0 1 1 1.06-1.06l3.22 3.22V3a.75.75 0 0 1 .75-.75Zm-9 13.5a.75.75 0 0 1 .75.75v2.25a1.5 1.5 0 0 0 1.5 1.5h13.5a1.5 1.5 0 0 0 1.5-1.5V16.5a.75.75 0 0 1 1.5 0v2.25a3 3 0 0 1-3 3H5.25a3 3 0 0 1-3-3V16.5a.75.75 0 0 1 .75-.75Z"
                    clip-rule="evenodd" />
            </svg>
        </button>
    </div>

    <div class="w-full max-w-5xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">
        <form @submit.prevent="handleUpload">
            <div class="space-y-12">
                <!-- Upload file -->
                <div class="col-span-full">
                    <h3 class="text-base/7 font-semibold text-gray-900">Upload finished checklist</h3>
                    <label for="file-upload"
                        class="flex flex-col items-center justify-center w-full h-32 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100 transition">
                        <div class="flex flex-col items-center justify-center pt-5 pb-6">
                            <svg class="w-8 h-8 mb-2 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5"
                                viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round"
                                    d="M3 16.5V19a2.5 2.5 0 002.5 2.5h13a2.5 2.5 0 002.5-2.5v-2.5M16.5 12.75L12 17.25m0 0l-4.5-4.5M12 17.25V4.5" />
                            </svg>
                            <p class="mb-2 text-sm text-gray-500">
                                <span class="font-semibold">Click to upload</span> or drag and drop
                            </p>
                            <p class="text-xs text-gray-400">.xls files only</p>
                        </div>
                        <input required id="file-upload" type="file" class="hidden" @change="onFileChange"
                            accept=".xls" />
                    </label>
                    <div v-if="selectedFile" class="mt-4 text-sm text-gray-600">Filename: {{ selectedFile.name }}</div>
                </div>

                <!-- User messages -->
                <div>
                    <p v-if="successMessage" class="text-sm text-emerald-500 font-semibold">{{ successMessage }}</p>
                    <p v-if="errorMessage" class="text-sm text-red-400 font-semibold">{{ errorMessage }}</p>
                </div>

                <!-- submit Button -->
                <button type="submit"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Upload
                </button>

            </div>
        </form>
    </div>
</template>