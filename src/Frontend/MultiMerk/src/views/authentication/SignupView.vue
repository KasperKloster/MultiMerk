<script setup>
import axios from 'axios';
import { inject, ref } from 'vue';

const apiUrl = inject('apiUrl');
const username = ref('');
const email = ref('');
const password = ref('');

const successMessage = ref('');
const errorMessage = ref('');

// submit
const handleSubmit = async () => {
  try {
    const response = await axios.post(`${apiUrl}/api/auth/signup`, {
      username : username.value,
      email : email.value,
      password : password.value
    });

    // Set success message
    successMessage.value = 'User created successfully!';
    // Clear the form fields
    username.value = '';
    email.value = '';
    password.value = '';
    
  } catch (err) {
    errorMessage.value = 'Signup failed. Please try again..';
    console.error('Error during signup:', err);    
  }
}

</script>

<template>
    <header class="bg-white shadow-sm">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">Signup</h1>
      </div>
    </header>
    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <div class="flex min-h-full flex-col justify-center my-12 px-6 py-12 lg:px-8 shadow-md rounded-md">
          <div class="sm:mx-auto sm:w-full sm:max-w-sm">    
            <h2 class="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900">Create user</h2>            
            <div v-if="successMessage" class="mb-4 text-green-600 text-center font-semibold text-2xl">              
              {{ successMessage }}
            </div>

            <div v-if="errorMessage" class="mb-4 text-red-600 text-center font-semibold text-2xl">              
              {{ errorMessage }}
            </div>
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
                </div>
                <div class="mt-2">
                  <input v-model="password" type="password" name="password" id="password" autocomplete="current-password" required class="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6">
                </div>
              </div>

              <div>                
                <button type="submit" class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 cursor-pointer">Create user</button>
              </div>
            </form>
          </div>
        </div>        
      </div>
    </main>
</template>