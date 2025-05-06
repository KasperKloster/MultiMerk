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

const handleUpload = async () => {
    // Reset messages
    successMessage.value = '';
    errorMessage.value = '';

    // Setting formdata
    const formData = new FormData();
    // Append
    formData.append('weeklistId', weeklistId);

    try {
        const response = await api.post(`/weeklist/admin/create-translations`, formData);
        successMessage.value = `Success`
        console.info("Upload success");

    } catch (error) {
        console.log(error);
        errorMessage.value = `Upload failed: ${error.response.data}`;
    }
};

</script>

<template>
    <Header title="Create Translations " />
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
        <form @submit.prevent="handleUpload">
            <div class="space-y-12">

                <!-- submit Button -->
                <button type="submit"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Upload
                </button>

            </div>
        </form>
    </div>
</template>