<script setup>
import Header from '@/components/layout/Header.vue';
import axios from 'axios';
import { inject, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const apiUrl = inject('apiUrl');
const username = ref('');
const email = ref('');
const password = ref('');
const errorMessage = ref('');

const handleSubmit = async () => {
  try {
    const response = await axios.post(`${apiUrl}/api/auth/login`, {
      username : username.value,
      email : email.value,
      password : password.value
    });
    // Sets token
    localStorage.setItem('multimerk_accessToken', response.data.accessToken); // Save access token
    localStorage.setItem('multimerk_refreshToken', response.data.refreshToken); // Save refresh token (or use HTTP-only cookie)
    // redirect to dashboard
    router.push('/dashboard'); // Redirect to login page after logout
    
  } catch (err) {
    errorMessage.value = 'Login failed. Please try again..';
    console.error('Error during signup:', err);    
  }
}


</script>

<template>
  <Header title="Login" />

    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <div class="flex min-h-full flex-col justify-center my-12 px-6 py-12 lg:px-8 shadow-md rounded-md">
          <div class="sm:mx-auto sm:w-full sm:max-w-sm">    
            <h2 class="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900">Login to your account</h2>
          </div>

          <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
            <form class="space-y-6" @submit.prevent="handleSubmit">
              
              <div>
                <label for="username" class="block text-sm/6 font-medium text-gray-900">Username</label>
                <div class="mt-2">
                  <input v-model="username" type="text" name="username" id="username" autocomplete="username" required class="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6">
                </div>
              </div>

              <div>
                <label for="email" class="block text-sm/6 font-medium text-gray-900">Email address</label>
                <div class="mt-2">
                  <input v-model="email" type="email" name="email" id="email" autocomplete="email" required class="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6">
                </div>
              </div>

              <div>
                <div class="flex items-center justify-between">
                  <label for="password" class="block text-sm/6 font-medium text-gray-900">Password</label>
                  <!-- <div class="text-sm">
                    <a href="#" class="font-semibold text-indigo-600 hover:text-indigo-500">Forgot password?</a>
                  </div> -->
                </div>
                <div class="mt-2">
                  <input v-model="password" type="password" name="password" id="password" autocomplete="current-password" required class="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6">
                </div>
              </div>

              <div>
                <button type="submit" class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 cursor-pointer">Login</button>
              </div>
            </form>
          </div>
        </div>          
      </div>
    </main>
</template>