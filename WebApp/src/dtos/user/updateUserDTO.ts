export interface UpdateProfileDTO {
  username?: string;
  firstName?: string;
  lastName?: string;
}

export interface ChangePasswordDTO {
  currentPassword: string;
  newPassword: string;
}