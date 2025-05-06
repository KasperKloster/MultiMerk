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


const getXlsFile = async () => {
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';

    // Setting formdata
    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(
            '/weeklist/warehouse/get-warehouse-list',
            formData,
            { responseType: 'blob' }
        );

        // Default filename    
        let fileName = `${weeklistId}-Warehouselist.xls`;

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

const MarkAsTaskComplete = async () => {    
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';

    // Setting formdata
    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    try {                                
        const response = await api.post('/weeklist/warehouse/mark-as-complete', formData)
        console.log(response);
        successMessage.value = 'Task is completed'
    } catch (error) {
        console.error(error);
        errorMessage.value = "Issue with completing task"
    }
};

</script>

<template>
    <Header title="Insert Warehouse list " />
    <BackToWeeklistLink />
    <div class="w-full max-w-5xl mx-auto p-6 bg-white rounded-xl shadow-md space-y-4">
        <button @click="getXlsFile()" type="button"
            class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-10 py-2.5 text-center inline-flex items-center me-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 cursor-pointer">
            Download List
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="size-6 ml-2">
                <path fill-rule="evenodd"
                    d="M12 2.25a.75.75 0 0 1 .75.75v11.69l3.22-3.22a.75.75 0 1 1 1.06 1.06l-4.5 4.5a.75.75 0 0 1-1.06 0l-4.5-4.5a.75.75 0 1 1 1.06-1.06l3.22 3.22V3a.75.75 0 0 1 .75-.75Zm-9 13.5a.75.75 0 0 1 .75.75v2.25a1.5 1.5 0 0 0 1.5 1.5h13.5a1.5 1.5 0 0 0 1.5-1.5V16.5a.75.75 0 0 1 1.5 0v2.25a3 3 0 0 1-3 3H5.25a3 3 0 0 1-3-3V16.5a.75.75 0 0 1 .75-.75Z"
                    clip-rule="evenodd" />
            </svg>
        </button>

        <button 
            @click="MarkAsTaskComplete()" 
            type="button"
            class="text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-10 py-2.5 text-center inline-flex items-center me-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 cursor-pointer">
            Mark as Complete
        </button>
    </div>
</template>