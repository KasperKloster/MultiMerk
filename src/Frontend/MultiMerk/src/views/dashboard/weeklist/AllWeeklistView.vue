<script setup>
import Header from '@/components/layout/Header.vue';
import { useRouter } from 'vue-router';
import { ref, onMounted } from 'vue';
import { getUserRole } from '@/utils/isUserLoggedIn';
import api from '@/utils/api';

const router = useRouter();
const weeklists = ref([]);
const loading = ref(true);
const currentUserRole = ref();

onMounted(async () => {
    currentUserRole.value = getUserRole();

    try {
        const response = await api.get(`/weeklist/all`);
        weeklists.value = response.data;
    } catch (err) {
        console.error(err);
    } finally {
        loading.value = false;
    }
});

const isDisabled = ((taskStatus, taskUserRole) => {
    const role = currentUserRole.value?.toLowerCase() || '';
    const assignedRole = taskUserRole?.toLowerCase() || '';
    const status = taskStatus?.toLowerCase() || '';

    return (
        (role !== 'admin' && role !== assignedRole) ||
        status !== 'ready' && status !== 'in progress'
    );
});


const goToTask = (taskId, weeklistId) => {
    
    if (taskId === 1) {
        router.push({ name: 'assign-ean', params: { id: weeklistId } });
    }
    if (taskId === 2) {
        router.push({ name: 'get-ai-content', params: { id: weeklistId } });
    }
    if (taskId === 3) {        
        router.push({ name: 'assign-location', params: { id: weeklistId } });
    }    
    if (taskId === 4) {
        router.push({ name: 'assign-qty', params: { id: weeklistId } });
    }
    if (taskId === 5) {
        router.push({ name: 'upload-ai-content', params: { id: weeklistId } });
    }
    if (taskId === 6) {
        router.push({ name: 'create-final-list', params: { id: weeklistId } });
    }
    if (taskId === 7) {
        router.push({ name: 'import-product-list', params: { id: weeklistId } });
    }
    if (taskId === 8) {
        router.push({ name: 'create-translations', params: { id: weeklistId } });
    }                        
};

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
                            Get AI Content
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

                    <tr v-for="week in weeklists" :key="week.id"
                        class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 border-gray-200 hover:bg-gray-50 dark:hover:bg-gray-600">
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
                            {{ task.weeklistTaskId }}
                            <div class="flex flex-col">
                                <div class="flex items-center">
                                    <div class="h-2.5 w-2.5 rounded-full me-2" :class="{
                                        'bg-gray-300': task.status?.status === 'Awaiting',
                                        'bg-blue-300': task.status?.status === 'Ready',
                                        'bg-amber-300': task.status?.status === 'In Progress',
                                        'bg-green-300': task.status?.status === 'Done'
                                    }"></div> {{ task.status?.status }}
                                </div>
                                <div class="py-3">
                                    <p class="font-small text-gray-500"><em>Owner: </em><b>{{
                                        task.assignedUser?.name }}</b></p>
                                </div>

                                <div>
                                    <button @click="goToTask(task.weeklistTaskId, week.id)" type="button" class="w-full px-3 py-2 text-xs font-medium text-center text-white rounded-lg
                                                bg-emerald-400 focus:ring-4 focus:outline-none focus:ring-emerald-300
                                                dark:bg-blue-600 dark:hover:bg-emerald-700 dark:focus:ring-emerald-800
                                                hover:bg-emerald-800" :class="{
                                                    'cursor-not-allowed opacity-50': isDisabled(task.status?.status, task.assignedUser?.userRole),
                                                    'cursor-pointer': !isDisabled(task.status?.status, task.assignedUser?.userRole)
                                                }"
                                        :disabled="isDisabled(task.status?.status, task.assignedUser?.userRole)">
                                        Go
                                    </button>
                                </div>
                            </div>
                        </td>

                    </tr>
                </tbody>
            </table>
        </div>
    </div>

</template>