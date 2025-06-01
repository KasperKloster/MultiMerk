<script setup>
import { ArrowLeftIcon, XMarkIcon } from '@heroicons/vue/24/solid';
import { ref, watch } from 'vue';

const props = defineProps({
  message: String
});

const isVisible = ref(true);

// Hide when there's no message
watch(() => props.message, (newVal) => {
  isVisible.value = !!newVal;
});

const dismiss = () => {
  isVisible.value = false;
};
</script>

<template>
    <div
        v-if="isVisible" 
        id="alert-additional-content-3"
        class="p-4 mb-4 text-green-800 border border-green-300 rounded-lg bg-green-50 dark:bg-gray-800 dark:text-green-400 dark:border-green-800"
        role="alert">
        <div class="flex items-center">
            <svg class="shrink-0 w-4 h-4 me-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                viewBox="0 0 20 20">
                <path
                    d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z" />
            </svg>
            <span class="sr-only">Info</span>
            <h3 class="text-lg font-medium">Done!</h3>              
            <button 
              @click="dismiss" 
              type="button" class="ms-auto -mx-1.5 -my-1.5 bg-green-50 text-red-500 rounded-lg focus:ring-2 focus:ring-green-400 p-1.5 hover:bg-red-200 inline-flex items-center justify-center h-8 w-8 dark:bg-gray-800 dark:text-green-400 dark:hover:bg-gray-700 cursor-pointer" aria-label="Close">            
                <XMarkIcon class="w-6 h-6" />
            </button>
        </div>
        <div class="mt-2 mb-4 text-sm">
            {{ message }}
        </div>
        <div class="flex">
            <a href="/weeklist/all"
                class="px-5 py-2.5 text-center inline-flex items-center me-2 text-green-800 bg-transparent border border-green-800 hover:bg-green-900 hover:text-white focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-xs dark:hover:bg-green-600 dark:border-green-600 dark:text-green-400 dark:hover:text-white dark:focus:ring-green-800 cursor-pointer">
                <ArrowLeftIcon class="w-5 h-5 mr-2 text-green-800" />
                Back to all Weeklists
            </a>
        </div>
    </div>
</template>


