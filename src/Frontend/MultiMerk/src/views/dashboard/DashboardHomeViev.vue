<script setup>
import { onMounted } from 'vue';
import { isAuthenticated } from '@/utils/isUserLoggedIn';
import Header from '@/components/layout/Header.vue';



onMounted(() => {
    // Check if the user is logged in and get their role
    const role = isAuthenticated();
    // if (role === 'admin') {
    //     console.log('User is an admin');
    // } else if (role === 'user') {
    //     console.log('User is a regular user');
    // } else {
    //     console.log('User is not logged in');
    // }    
});


</script>

<template>
    <Header title="Dashboard Home" />
</template>