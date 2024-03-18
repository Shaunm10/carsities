import { Auction, PagedResults } from "@/types";

type State = {
    auctions: Auction[];
    totalCount: number;
    pageCount: number;
}

type Actions = {
    setData: (data: PagedResults<Auction>) => void;
    setCurrentPrice: (auctionId: string, amount: number) => void;
}

const initialState: State = {
    auctions: [],
    totalCount: 0,
    pageCount: 0
};
