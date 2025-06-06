<script setup>
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import Header from '@/components/layout/Header.vue';
import BackToWeeklistLink from '@/components/layout/BackToWeeklistLink.vue';
import TaskDescriptionText from '@/components/layout/TaskDescriptionText.vue';
import ErrorAlert from '@/components/layout/alerts/ErrorAlert.vue';
import SuccessAlert from '@/components/layout/alerts/SuccessAlert.vue';
import api from '@/utils/api';

const route = useRoute();
const weeklistId = route.params.id;

const successMessage = ref('');
const errorMessage = ref('');
const selectedFile = ref(null);

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

    // Check if file is selected
    if (!selectedFile.value) return;

    // Setting formdata
    const formData = new FormData();
    // Getting the file
    formData.append('file', selectedFile.value);
    // Append
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(`/weeklist/admin/insert-out-of-stock`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        successMessage.value = `Stock has been updated`
        console.info("Upload success");
        selectedFile.value = null;

    } catch (error) {
        console.log(error);
        errorMessage.value = `Upload failed: ${error.response.data}`;
    }
};
</script>

<template>
    <Header title="Upload Out of Stock list" />
    <BackToWeeklistLink />
    <TaskDescriptionText description="Get shortage list from supplier, and upload it here." />

    <div class="w-full max-w-5xl mx-auto">
        <div v-if="successMessage">
            <SuccessAlert :message="successMessage" />
        </div>
        <div v-if="errorMessage">
            <ErrorAlert :message="errorMessage" />
        </div>
    </div>

    <div class="w-full max-w-5xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">
        <form @submit.prevent="handleUpload">
            <div class="space-y-12">
                <!-- Upload file -->
                <div class="col-span-full">
                    <h3 class="text-base/7 font-semibold text-gray-900">Upload Out of Stock list</h3>
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
                <button type="submit" :disabled="!selectedFile"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Upload
                </button>

            </div>
        </form>
    </div>
</template>