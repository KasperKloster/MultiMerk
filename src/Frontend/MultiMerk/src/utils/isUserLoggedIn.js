//  Checks if user is logged in by verifying the JWT token stored in localStorage
import { jwtDecode } from "jwt-decode";

/**
 * Checks if the user is logged in by verifying the JWT token in localStorage.
 * @returns {boolean} True if the user is authenticated, otherwise false.
 */
export function isUserLoggedIn() {
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
            return false;
        }
    } catch (error) {
        // Other errors
        console.error("Error decoding token:", error);        
        return false;
    }
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

export function isAuthenticated(){
    const isAuth = isUserLoggedIn();
    // Check if user is logged in, redirect to login if not
    if(!isAuth){
        console.warn("User not authenticatd. Redirecting to login...");
        window.location.href = "/login";        
    }
    // Get and return user role if they are authenticated
    const role = getUserRole();
    return role;
}