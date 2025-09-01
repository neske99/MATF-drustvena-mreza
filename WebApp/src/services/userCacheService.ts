import { userStore } from '@/stores/user';
import type { UserDetailDTO } from '@/dtos/user/userDetailDTO';

class UserCacheService {
    private cache: Map<string, UserDetailDTO> = new Map();
    private pendingRequests: Map<string, Promise<UserDetailDTO | null>> = new Map();

    async getUserByUsername(username: string): Promise<UserDetailDTO | null> {
        // Check cache first
        if (this.cache.has(username)) {
            return this.cache.get(username)!;
        }

        // Check if there's already a pending request for this user
        if (this.pendingRequests.has(username)) {
            return await this.pendingRequests.get(username)!;
        }

        // Create new request
        const request = this.fetchUserFromApi(username);
        this.pendingRequests.set(username, request);

        try {
            const user = await request;
            if (user) {
                this.cache.set(username, user);
            }
            return user;
        } finally {
            this.pendingRequests.delete(username);
        }
    }

    private async fetchUserFromApi(username: string): Promise<UserDetailDTO | null> {
        try {
            const user = await userStore().GetUser(username);
            return user;
        } catch (error) {
            console.log(`Could not fetch user ${username}:`, error);
            return null;
        }
    }

    // Clear cache when needed
    clearCache() {
        this.cache.clear();
    }

    // Update cache when user data changes
    updateUserCache(username: string, userData: UserDetailDTO) {
        this.cache.set(username, userData);
    }
}

// Create singleton instance
export const userCacheService = new UserCacheService();