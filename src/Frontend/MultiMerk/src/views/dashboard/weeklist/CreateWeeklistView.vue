<script setup>
import { onMounted } from 'vue';
import { isAuthenticated } from '@/utils/isUserLoggedIn';
import Header from '@/components/layout/Header.vue';


onMounted(() => {
    // Check if the user is logged in and get their role
    const role = isAuthenticated();
    // if (role === 'admin') {
    //     console.log('User is an admin');
    // } else if (role === 'user') {
    //     console.log('User is a regular user');
    // } else {
    //     console.log('User is not logged in');
    // }    
});


</script>

<template>
    <Header title="Create weeklist" />

    <div class="max-w-md mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">
        <h2 class="text-xl font-semibold text-gray-700">Upload CSV File</h2>

        <form @submit.prevent="handleUpload">
            <label for="file-upload"
                class="flex flex-col items-center justify-center w-full h-32 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100 transition">
                <div class="flex flex-col items-center justify-center pt-5 pb-6">
                    <svg class="w-8 h-8 mb-2 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M3 16.5V19a2.5 2.5 0 002.5 2.5h13a2.5 2.5 0 002.5-2.5v-2.5M16.5 12.75L12 17.25m0 0l-4.5-4.5M12 17.25V4.5" />
                    </svg>
                    <p class="mb-2 text-sm text-gray-500">
                        <span class="font-semibold">Click to upload</span> or drag and drop
                    </p>
                    <p class="text-xs text-gray-400">CSV files only</p>
                </div>
                <input id="file-upload" type="file" class="hidden" @change="onFileChange" accept=".csv" />
            </label>

            <div v-if="selectedFile" class="mt-4 text-sm text-gray-600">
                Selected: {{ selectedFile.name }}
            </div>
            
            <button type="submit" :disabled="!selectedFile"
                class="mt-4 w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300">
                Upload
            </button>
        </form>
    </div>

</template>