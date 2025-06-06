/**
* Checks if user is logged in by verifying the JWT token stored in localStorage
*/
import api from '@/utils/api';
import { jwtDecode } from "jwt-decode";

export async function isUserLoggedIn() {
    // Get token from localStorage
    const token = localStorage.getItem("multimerk_accessToken");    
    // Not token is found
    if (!token) {      
        return false;
    }
        
    try {
        const decoded = jwtDecode(token);
        const currentTime = Math.floor(Date.now() / 1000); // Convert to seconds    
        // Token is valid if it has not expired
        if (decoded.exp > currentTime) {            
            return true;
        } else {
            // Token is expired
            return await refreshToken();                    
        }
    } catch (error) {
        // Other errors
        console.error("Error decoding token:", error);        
        return false;
    }
}

export async function isAuthenticated(){
    const isAuth = await isUserLoggedIn();
    // Check if user is logged in, redirect to login if not
    if(!isAuth){
        console.warn("User not authenticated. Redirecting to login...");
        window.location.href = "/login";        
    }
    // Get and return user role if they are authenticated
    const role = getUserRole();
    return role;
}

// Get user role from token
export function getUserRole() {
    const token = localStorage.getItem("multimerk_accessToken");
    if (!token) return null;
    try {
        // Decode the token to get user role
        const decoded = jwtDecode(token);                
        // Check if the token has a role property and return it in lowercase
        return decoded.role ? decoded.role.toLowerCase() : null;        
    } catch (error) {
        console.error("Error decoding token:", error);
        return null;
    }
}

export async function refreshToken() {
    const accessToken = localStorage.getItem("multimerk_accessToken");
    const refreshToken = localStorage.getItem("multimerk_refreshToken");

    if (!refreshToken || !accessToken) {
        console.warn("Missing token(s).");
        return false;
    }

    try {
        const response = await api.post("/auth/token/refresh", {
            accessToken: accessToken,
            refreshToken: refreshToken
        });

        if (response.status === 200) {
            const newToken = response.data;
            localStorage.setItem("multimerk_accessToken", newToken.accessToken);
            localStorage.setItem("multimerk_refreshToken", newToken.refreshToken);
            console.log("Token refreshed successfully.");
            return true;
        } else {
            console.error("Failed to refresh token:", response.data);
            return false;
        }
    } catch (error) {
        console.error("Error refreshing token:", error);
        return false;
    }
}