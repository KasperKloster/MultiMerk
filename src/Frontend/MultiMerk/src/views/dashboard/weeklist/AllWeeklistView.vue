<script setup>
import Header from '@/components/layout/Header.vue';
import { inject, ref, onMounted } from 'vue';
import axios from 'axios';
const apiUrl = inject('apiUrl');

const weeklists = ref([]);
const loading = ref(true);


onMounted(async () => {
  try {    
    const response = await axios.get(`${apiUrl}/api/weeklist/all`);
    weeklists.value = response.data;
    console.log(weeklists);
  } catch (err) {
    console.error(err);    
  } finally {
    loading.value = false;    
  }
});


</script>

<template>
    <Header title="Weeklist overview" />
    
    <div class="w-full mx-auto mt-2 p-6 bg-white">   
        <div v-if="loading">Loading weeklists...</div>

        <div v-else class="relative overflow-x-auto shadow-md sm:rounded-lg">
            <table class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" class="px-6 py-3">
                            Weeklist Number
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Order Number
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Supplier
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Assign EAN
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Create AI Content
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Assign Location
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Assign Correct Quantity
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Upload AI Content
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Create Final List
                        </th>                        
                        <th scope="col" class="px-6 py-3">
                            Import Product List
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Create Translations
                        </th>                                                
                    </tr>
                </thead>
                <tbody>

                    <tr v-for="week in weeklists" :key="week.id" class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 border-gray-200 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">
                            {{ week.number }}
                        </th>
                        <td class="px-6 py-4">
                            {{ week.orderNumber }}
                        </td>
                        <td class="px-6 py-4">
                            {{ week.supplier }}                            
                        </td>   

                        <td v-for="task in week.weeklistTasks" :key="task.weeklistTaskId" class="px-6 py-4">
                            <div class="flex flex-col">                                
                                <div class="flex items-center">
                                    <div class="h-2.5 w-2.5 rounded-full me-2"
                                    :class="{
                                        'bg-gray-300': task.status?.status === 'Awaiting',
                                        'bg-blue-300': task.status?.status === 'Ready',
                                        'bg-amber-300': task.status?.status === 'In Progress',
                                        'bg-green-300': task.status?.status === 'Done'
                                    }"
                                    ></div> {{task.status?.status}}                                
                                </div>
                                <div class="py-3">
                                    <p class="font-small text-gray-500"> leslie@flowbite.com</p>
                                </div>
                                
                                <div>
                                    <button type="button" class="w-full px-3 py-2 text-xs font-medium text-center text-white bg-emerald-400 
                                    rounded-lg hover:bg-emerald-800 focus:ring-4 focus:outline-none focus:ring-emerald-300 dark:bg-blue-600 dark:hover:bg-emerald-700 dark:focus:ring-emerald-800 cursor-pointer">Go</button>
                                </div>
                            </div>
                        </td> 

                    </tr>
                </tbody>
            </table>
        </div>
    </div>

</template>