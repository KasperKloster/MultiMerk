<script setup>
import NavBar from './components/navigation/NavBar.vue';
import { RouterView } from 'vue-router';
</script>

<template>
    <div class="min-h-full">      
      <NavBar />
      <RouterView />      
    </div>
</template>