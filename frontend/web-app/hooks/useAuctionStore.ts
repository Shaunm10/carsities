import { Auction, PagedResults } from '@/types';
import { create } from 'zustand';

type State = {
  auctions: Auction[];
  totalCount: number;
  pageCount: number;
};

type Actions = {
  setData: (data: PagedResults<Auction>) => void;
  setCurrentPrice: (auctionId: string, amount: number) => void;
};

const initialState: State = {
  auctions: [],
  totalCount: 0,
  pageCount: 0,
};

/**
 * The export that contains the `Reducer` and selector functionality
 */
export const useAuctionStore = create<State & Actions>((set) => ({
  ...initialState,
  setData: (data: PagedResults<Auction>) => {
    set(() => ({
      auctions: data.results,
      totalCount: data.totalCount,
      pageCount: data.pageCount,
    }));
  },
  setCurrentPrice: (auctionId: string, amount: number) => {
    set((state) => ({
      ...state,
      auctions: state.auctions.map((auction) =>
        auction.id === auctionId
          ? { ...auction, currentHighBid: amount }
          : auction
      ),
    }));
  },
}));
