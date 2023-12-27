export type PagedResults<T> = {
  results: T[];
  pageCount: number;
  totalCount: number;
};

export type Auction = {
  reservePrice: number;
  seller: string;
  winner?: string;
  soldAmount: number;
  currentHighBid?: number;
  createdAt: string;
  updatedAt: string;
  status: string;
  auctionEnd: string;
  make: string;
  model: string;
  year: number;
  color: string;
  mileage: number;
  imageUrl: string;
  id: string;
};
