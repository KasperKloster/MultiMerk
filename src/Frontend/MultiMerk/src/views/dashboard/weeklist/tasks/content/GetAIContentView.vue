<script setup>
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import api from '@/utils/api';
import Header from '@/components/layout/Header.vue';
import BackToWeeklistLink from '@/components/layout/BackToWeeklistLink.vue';
import ErrorAlert from '@/components/layout/alerts/ErrorAlert.vue';
import SuccessAlert from '@/components/layout/alerts/SuccessAlert.vue';

const route = useRoute();
const weeklistId = route.params.id;
const successMessage = ref('');
const errorMessage = ref('');

const getCsvFile = async () => {
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';

    // Setting formdata
    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(
            '/weeklist/content/get-products-ready-for-ai-content',
            formData,
            { responseType: 'blob' }
        );

        // Default filename
        let fileName = `products-weeklist-${weeklistId}.csv`;

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

        const blob = new Blob([response.data], { type: 'text/csv' });
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

</script>

<template>
    <Header title="Get AI Content" />
    <BackToWeeklistLink />
    <div class="w-full max-w-5xl mx-auto">
        <div v-if="successMessage">
            <SuccessAlert :message="successMessage" />
        </div>
        <div v-if="errorMessage">
            <ErrorAlert :message="errorMessage" />
        </div>
    </div>

    <div class="w-full max-w-5xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">
        <form>
            <div class="space-y-12">
                <!-- submit Button -->
                <button @click="getCsvFile" type="submit"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Get
                </button>

            </div>
        </form>
    </div>

</template>