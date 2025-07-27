import type { commentDTO } from "./commentDTO";
import type { userDTO } from "./userDTO";

export interface postDTO{
  id: number,
  text: string,
  userId: number,
  user:userDTO,
  comments?: commentDTO[],


};