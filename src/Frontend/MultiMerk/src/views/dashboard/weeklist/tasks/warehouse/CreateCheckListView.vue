<script setup>
import { downloadFile } from '@/utils/fileDownloader';
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import api from '@/utils/api';
import Header from '@/components/layout/Header.vue';
import BackToWeeklistLink from '@/components/layout/BackToWeeklistLink.vue';
import TaskDescriptionText from '@/components/layout/TaskDescriptionText.vue';
import ErrorAlert from '@/components/layout/alerts/ErrorAlert.vue';
import SuccessAlert from '@/components/layout/alerts/SuccessAlert.vue';

const route = useRoute();
const weeklistId = route.params.id;
const successMessage = ref('');
const errorMessage = ref('');
const selectedFile = ref(null);

const downloadXlsFile = async () => {
    successMessage.value = '';
    errorMessage.value = '';

    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    await downloadFile({
        url: '/weeklist/warehouse/get-checklist',
        formData,
        defaultFileName: `${weeklistId}-Checklist.xls`,
        mimeType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        onSuccess: () => successMessage.value = 'Download successful',
        onError: (msg) => errorMessage.value = `Download failed: ${msg}`
    });
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
        successMessage.value = `Qty and Location has been updated`
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
    <TaskDescriptionText description="Download the file. Give correct quantity, and location. Then upload it here again" />
    
    <div class="w-full max-w-5xl mx-auto">
        <div v-if="successMessage">
            <SuccessAlert :message="successMessage" />
        </div>
        <div v-if="errorMessage">
            <ErrorAlert :message="errorMessage" />
        </div>
    </div>

    <div class="w-full max-w-5xl mx-auto p-6 bg-white rounded-xl shadow-md space-y-4">
        <button @click="downloadXlsFile()" type="button"
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
                                <span class="font-semibold">Click to upload</span>
                            </p>
                            <p class="text-xs text-gray-400">.xls files only</p>
                        </div>
                        <input required id="file-upload" type="file" class="hidden" @change="onFileChange"
                            accept=".xls" />
                    </label>
                    <div v-if="selectedFile" class="mt-4 text-sm text-gray-600">Filename: {{ selectedFile.name }}</div>
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