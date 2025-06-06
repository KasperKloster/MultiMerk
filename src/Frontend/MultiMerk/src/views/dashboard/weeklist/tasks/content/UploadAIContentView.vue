<script setup>
import { uploadFile } from '@/utils/fileUploader';
import { useRoute } from 'vue-router';
import { ref } from 'vue';
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

const onFileChange = (event) => {
    const file = event.target.files[0];

    if (file && file.type === 'text/csv') {
        selectedFile.value = file;
    } else {
        errorMessage.value = 'Please, upload an .csv file..';
        console.error('Please, upload an .csv file')
        selectedFile.value = null;
    }
}

const handleUpload = async () => {
    successMessage.value = '';
    errorMessage.value = '';    
    if (!selectedFile.value) return;

    const formData = new FormData();
    formData.append('file', selectedFile.value);    
    formData.append('weeklistId', weeklistId);

    await uploadFile({
        url: '/weeklist/content/upload-ai-content',
        formData,
        onSuccess: (data) => {
            successMessage.value = `Products has been updated`;
            selectedFile.value = null;
        },
        onError: (msg) => {
            errorMessage.value = `Upload failed: ${msg}`;
        },
    });
};

</script>

<template>
    <Header title="Upload AI Content " />
    <BackToWeeklistLink />
    <TaskDescriptionText description="Upload products content created by AI" />
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
                    <h3 class="text-base/7 font-semibold text-gray-900">Upload AI Content</h3>
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
                            <p class="text-xs text-gray-400">.csv files only</p>
                        </div>
                        <input required id="file-upload" type="file" class="hidden" @change="onFileChange" accept=".csv" />
                    </label>
                    <div v-if="selectedFile" class="mt-4 text-sm text-gray-600">Filename: {{ selectedFile.name }}</div>
                </div>

            <div class="space-y-12">
                <!-- submit Button -->
                <button type="submit"
                    class="mt-5 w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Upload
                </button>

            </div>
        </form>
    </div>
</template>