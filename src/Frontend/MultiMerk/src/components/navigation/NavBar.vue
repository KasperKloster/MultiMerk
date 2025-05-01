<script setup>
import api from '@/utils/api';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { isUserLoggedIn, getUserRole } from '@/utils/isUserLoggedIn';
import { Disclosure, DisclosureButton, DisclosurePanel, Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/vue'
import { Bars3Icon, BellIcon, XMarkIcon } from '@heroicons/vue/24/outline'
import { ChevronDownIcon, UserIcon } from '@heroicons/vue/20/solid'

const isLoggedIn = ref(false);
const userRole = ref(null);
const router = useRouter();

// Display this menu to all users
const navigation = ref([{ name: 'Home', href: '/', current: false }])
const weeklistNavigation = ref([]);
const contentNavigation = ref([]);
const warehouseNavigation = ref([]);

onMounted(async () => {
  // Check if the user is logged in, and display content to all user types
  isLoggedIn.value = await isUserLoggedIn();
  userRole.value = await getUserRole();
  // userRole.value = 'warehouseworker'; // For testing purposes

  // If the user is not logged in, add additional navigation items    
  if (!isLoggedIn.value) {
    navigation.value.push(
      { name: 'Signup', href: '/signup', current: false },
      { name: 'Login', href: '/login', current: false });
  };

  // Display for logged in users
  if (isLoggedIn.value) {
    // for all logged in users
    navigation.value.push({ name: 'Dashboard', href: '/dashboard', current: false });
    weeklistNavigation.value.push({ name: 'All Weeklists', href: '/weeklist/all' });
  };

  // Menu items for freelancer
  if (userRole.value === 'admin' || userRole.value === 'freelancer') {
      weeklistNavigation.value.push({ name: 'Create a new Weeklist', href: '/weeklist/tasks/create', current: false });
  };

  // Menu items for Writer
  if (userRole.value === 'admin' || userRole.value === 'writer') {
      contentNavigation.value.push({ name: 'Create AI content', href: '/weeklist/tasks/content/create-ai-content', current: false });
      contentNavigation.value.push({ name: 'Upload AI content', href: '/weeklist/tasks/content/upload-ai-content', current: false });      
  };

  // Menu items for Warehouse
  if (userRole.value === 'admin' || userRole.value === 'warehouseworker'  || userRole.value === 'warehousemanager') {
      warehouseNavigation.value.push({ name: 'Assign Location and Quantity', href: '/weeklist/tasks/warehouse/assign-location-qty', current: false });            
  };

  // Menu items for admin only
  if (userRole.value === 'admin') {
    // weeklistNavigation.value.push({ name: 'Assign EAN', href: '/weeklist/tasks/admin/assign-ean', current: false });
    weeklistNavigation.value.push({ name: 'Create Final List', href: '/weeklist/tasks/admin/create-final-list', current: false });
    weeklistNavigation.value.push({ name: 'Create Translations', href: '/weeklist/tasks/admin/create-translations', current: false });
  };  

});

// Function to handle logout
const handleLogout = async () => {
  try {
    const response = await api.post(`/auth/token/revoke`);
    if (response.status === 200) {
      // Clear the access token and refresh token from local storage
      localStorage.removeItem('multimerk_accessToken');
      localStorage.removeItem('multimerk_refreshToken');
      // Update the logged-in status
      isLoggedIn.value = false;
      // Redirect to login page after logout
      router.push('/login');
    }

  } catch (error) {
    console.error('Error during logout:', error);
  }
};

</script>

<template>
  <Disclosure as="nav" class="bg-gray-800" v-slot="{ open }">
    <div class="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
      <div class="relative flex h-16 items-center justify-between">
        <div class="absolute inset-y-0 left-0 flex items-center sm:hidden">
          <!-- Mobile menu button-->
          <DisclosureButton
            class="relative inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-700 hover:text-white focus:ring-2 focus:ring-white focus:outline-hidden focus:ring-inset cursor-pointer"
            :class="{ 'bg-gray-900 text-white': open }">
            <span class="absolute -inset-0.5" />
            <span class="sr-only">Open main menu</span>
            <Bars3Icon v-if="!open" class="block size-6" aria-hidden="true" />
            <XMarkIcon v-else class="block size-6" aria-hidden="true" />
          </DisclosureButton>
        </div>
        <div class="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
          <div class="flex shrink-0 items-center">
            <img class="h-8 w-auto" src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
              alt="Your Company" />
          </div>
          <div class="hidden sm:ml-6 sm:block">

            <div class="flex space-x-4 items-center">
              <!-- Normal nav links -->
              <a v-for="item in navigation" :key="item.name" :href="item.href"
                :class="[item.current ? 'bg-gray-900 text-white' : 'text-gray-300 hover:bg-gray-700 hover:text-white', 'rounded-md px-3 py-2 text-sm font-medium']"
                :aria-current="item.current ? 'page' : undefined">
                {{ item.name }}
              </a>
              
              <!-- weeklistNavigation -->
              <Menu as="div" class="relative inline-block text-left" v-if="weeklistNavigation.length > 0">
                <div>
                  <MenuButton
                    class="inline-flex items-center justify-center gap-x-1.5 rounded-md bg-gray-800 px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 cursor-pointer">
                      Weeklist
                    <ChevronDownIcon class="-mr-1 size-5 text-gray-400" aria-hidden="true" />
                  </MenuButton>
                </div>
                <transition enter-active-class="transition ease-out duration-100"
                  enter-from-class="transform opacity-0 scale-95" 
                  enter-to-class="transform opacity-100 scale-100"
                  leave-active-class="transition ease-in duration-75" 
                  leave-from-class="transform opacity-100 scale-100"
                  leave-to-class="transform opacity-0 scale-95">
                    <MenuItems class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none">
                      <div class="py-1">
                        <MenuItem v-for="item in weeklistNavigation" :key="item.name" v-slot="{ active }">
                          <RouterLink
                            :to="item.href"
                            :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700','block px-4 py-2 text-sm']">
                            {{ item.name }}
                          </RouterLink>
                        </MenuItem>
                      </div>
                    </MenuItems>
                </transition>
              </Menu>

              <!-- contentNavigation  -->
              <Menu as="div" class="relative inline-block text-left" v-if="contentNavigation.length > 0">
                <div>
                  <MenuButton
                    class="inline-flex items-center justify-center gap-x-1.5 rounded-md bg-gray-800 px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 cursor-pointer">
                      Content
                    <ChevronDownIcon class="-mr-1 size-5 text-gray-400" aria-hidden="true" />
                  </MenuButton>
                </div>
                <transition enter-active-class="transition ease-out duration-100"
                  enter-from-class="transform opacity-0 scale-95" 
                  enter-to-class="transform opacity-100 scale-100"
                  leave-active-class="transition ease-in duration-75" 
                  leave-from-class="transform opacity-100 scale-100"
                  leave-to-class="transform opacity-0 scale-95">
                    <MenuItems class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none">
                      <div class="py-1">
                        <MenuItem v-for="item in contentNavigation" :key="item.name" v-slot="{ active }">
                          <RouterLink
                            :to="item.href"
                            :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700','block px-4 py-2 text-sm']">
                            {{ item.name }}
                          </RouterLink>
                        </MenuItem>
                      </div>
                    </MenuItems>
                </transition>
              </Menu>

              <!-- warehouseNavigation  -->
              <Menu as="div" class="relative inline-block text-left" v-if="warehouseNavigation.length > 0">
                <div>
                  <MenuButton
                    class="inline-flex items-center justify-center gap-x-1.5 rounded-md bg-gray-800 px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 cursor-pointer">
                      Warehouse
                    <ChevronDownIcon class="-mr-1 size-5 text-gray-400" aria-hidden="true" />
                  </MenuButton>
                </div>
                <transition enter-active-class="transition ease-out duration-100"
                  enter-from-class="transform opacity-0 scale-95" 
                  enter-to-class="transform opacity-100 scale-100"
                  leave-active-class="transition ease-in duration-75" 
                  leave-from-class="transform opacity-100 scale-100"
                  leave-to-class="transform opacity-0 scale-95">
                    <MenuItems class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none">
                      <div class="py-1">
                        <MenuItem v-for="item in warehouseNavigation" :key="item.name" v-slot="{ active }">
                          <RouterLink
                            :to="item.href"
                            :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700','block px-4 py-2 text-sm']">
                            {{ item.name }}
                          </RouterLink>
                        </MenuItem>
                      </div>
                    </MenuItems>
                </transition>
              </Menu>

            </div>
          </div>
        </div>

        <!-- For logged in users -->
        <div v-if="isLoggedIn">
          <div class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
            <button type="button"
              class="relative rounded-full bg-gray-800 p-1 text-gray-400 hover:text-white focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 focus:outline-hidden cursor-pointer">
              <span class="absolute -inset-1.5" />
              <span class="sr-only">View notifications</span>
              <BellIcon class="size-6" aria-hidden="true" />
            </button>

            <!-- Profile dropdown -->
            <Menu as="div" class="relative ml-3">
              <div>
                <MenuButton
                  class="relative flex rounded-full bg-gray-800 text-sm focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 focus:outline-hidden cursor-pointer">
                  <span class="absolute -inset-1.5" />
                  <span class="sr-only">Open user menu</span>                  
                  <UserIcon class="size-6 text-white" />      
                </MenuButton>
              </div>
              <transition enter-active-class="transition ease-out duration-100"
                enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100"
                leave-active-class="transition ease-in duration-75" leave-from-class="transform opacity-100 scale-100"
                leave-to-class="transform opacity-0 scale-95">
                <MenuItems
                  class="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black/5 focus:outline-hidden">
                  <MenuItem v-slot="{ active }">
                  <a href="#"
                    :class="[active ? 'bg-gray-100 outline-hidden' : '', 'block px-4 py-2 text-sm text-gray-700']">Your
                    Profile</a>
                  </MenuItem>
                  <MenuItem v-slot="{ active }">
                  <a href="#"
                    :class="[active ? 'bg-gray-100 outline-hidden' : '', 'block px-4 py-2 text-sm text-gray-700']">Settings</a>
                  </MenuItem>
                  <MenuItem v-slot="{ active }">
                  <a href="#"
                    :class="[active ? 'bg-gray-100 outline-hidden' : '', 'block px-4 py-2 text-sm text-gray-700']"
                    @click.prevet="handleLogout">Sign out</a>
                  </MenuItem>
                </MenuItems>
              </transition>
            </Menu>
          </div>
        </div>

      </div>
    </div>

    <DisclosurePanel class="sm:hidden">
      <div class="space-y-1 px-2 pt-2 pb-3">
        <DisclosureButton v-for="item in navigation" :key="item.name" as="a" :href="item.href"
          :class="[item.current ? 'bg-gray-900 text-white' : 'text-gray-300 hover:bg-gray-700 hover:text-white', 'block rounded-md px-3 py-2 text-base font-medium']"
          :aria-current="item.current ? 'page' : undefined">{{ item.name }}</DisclosureButton>
      </div>
    </DisclosurePanel>
  </Disclosure>
</template>