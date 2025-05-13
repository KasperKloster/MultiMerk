<script setup>
defineProps({
  description: {
    type: String,
    default: ''
  }
})
</script>

<template>
  <div class="w-full max-w-5xl mx-auto">
    <p class="text-gray-500 dark:text-gray-400">{{  description }}</p>
  </div>
</template>