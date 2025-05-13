<script setup>
import { downloadFile } from '@/utils/fileDownloader';
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import Header from '@/components/layout/Header.vue'
import BackToWeeklistLink from '@/components/layout/BackToWeeklistLink.vue';
import ErrorAlert from '@/components/layout/alerts/ErrorAlert.vue';
import SuccessAlert from '@/components/layout/alerts/SuccessAlert.vue';
import TaskDescriptionText from '@/components/layout/TaskDescriptionText.vue';

const route = useRoute();
const weeklistId = route.params.id;
const successMessage = ref('');
const errorMessage = ref('');

const downloadCsvFile = async () => {
    successMessage.value = '';
    errorMessage.value = '';

    const formData = new FormData();
    formData.append('weeklistId', weeklistId);

    await downloadFile({
        url: '/weeklist/admin/import-product-list',
        formData,
        defaultFileName: `${weeklistId}-Admin.csv`,        
        mimeType: 'application/zip',
        onSuccess: () => successMessage.value = 'Download successful - Import them to Magento and Shopify',
        onError: (msg) => errorMessage.value = `Download failed: ${msg}`
    });
};

</script>

<template>
    <Header title="Import Product List " />
    <BackToWeeklistLink />
    <TaskDescriptionText description="Download files; Then import them to Magento and Shopify" />
    
    <div class="w-full max-w-5xl mx-auto">        
        <div v-if="successMessage">
            <SuccessAlert :message="successMessage" />
        </div>
        <div v-if="errorMessage">
            <ErrorAlert :message="errorMessage" />
        </div>        
    </div>

    <div class="w-full max-w-5xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">        
        <form @submit.prevent="getImportLists">
            <div class="space-y-12">
                <!-- submit Button -->
                <button 
                    @click="downloadCsvFile"
                    type="submit"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Download files
                </button>

            </div>
        </form>
    </div>
</template>