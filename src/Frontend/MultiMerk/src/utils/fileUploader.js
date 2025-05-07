import api from '@/utils/api';

export async function uploadFile({ url, formData, onSuccess, onError }) {
    try {
        const response = await api.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });

        onSuccess?.(response.data);
    } catch (error) {
        console.error(error);
        const errorText = await error?.response?.data?.text?.() ?? 'An unknown upload error occurred.';
        onError?.(errorText);
    }
};