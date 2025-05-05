<script setup>
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import api from '@/utils/api';
import Header from '@/components/layout/Header.vue';

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
        const response = await api.post(`/weeklist/content/create-ai-content`, formData);        
        successMessage.value = `Success`
        console.info("Upload success");
        
    } catch (error) {
        console.log(error);
        errorMessage.value = `Upload failed: ${error.response.data}`;        
    }
};

</script>

<template>
    <Header title="Create AI Content" />

    <div class="w-full max-w-5xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md space-y-4">    
        <form @submit.prevent="handleUpload">
            <div class="space-y-12">


                <!-- User messages -->
                <div>
                    <p v-if="successMessage" class="text-sm text-emerald-500 font-semibold">{{ successMessage }}</p>                    
                    <p v-if="errorMessage" class="text-sm text-red-400 font-semibold">{{ errorMessage }}</p>
                </div>

                <!-- submit Button -->
                <button type="submit"
                    class="w-full inline-flex justify-center items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-md shadow focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:bg-indigo-300 cursor-pointer">
                    Get
                </button>

            </div>  
        </form>
    </div> 

</template>