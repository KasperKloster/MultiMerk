import api from '@/utils/api';

export async function downloadFile({ url, formData, defaultFileName, mimeType, onSuccess, onError }) {
    try {
        const response = await api.post(url, formData, { responseType: 'blob' });

        // Extract filename from content-disposition header
        let fileName = defaultFileName;
        const disposition = response.headers['content-disposition'];
        if (disposition) {
            const utf8Match = disposition.match(/filename\*\=UTF-8''(.+?)(?:;|$)/);
            const asciiMatch = disposition.match(/filename="?(.*?)"?(\s*;|$)/);

            if (utf8Match?.[1]) {
                fileName = decodeURIComponent(utf8Match[1]);
            } else if (asciiMatch?.[1]) {
                fileName = asciiMatch[1];
            }
        }

        const blob = new Blob([response.data], { type: mimeType });
        const blobUrl = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();

        onSuccess?.();
    } catch (error) {
        console.error(error);
        const errorText = await error?.response?.data?.text?.() ?? 'An unknown error occurred.';
        onError?.(errorText);
    }
};