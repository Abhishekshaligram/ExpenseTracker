import { IToken } from "./token.interface";

export interface ApiResponse {
    [x: string]: any;
    status: number;
    isStatus: boolean;
    data: any;
    metaData: {
      currentPage: number;
      totalFilteredCount: number;
      totalFilteredPage: number;
    };
    message: string;
    authToken?: IToken;
    user?: any;
  }