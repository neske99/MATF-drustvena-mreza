import type { userDTO } from "./userDTO";

export interface commentDTO{
  id: number,
  text: string,
  createdDate: string,
  user: userDTO
};