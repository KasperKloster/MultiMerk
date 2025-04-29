import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5020/api",
});

// Attach access token to requests
api.interceptors.request.use(config => {
    const token = localStorage.getItem("multimerk_accessToken");    
    if (token) {        
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => Promise.reject(error));

export default api;