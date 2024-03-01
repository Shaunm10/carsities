'use server';
import { Auction, PagedResults } from '@/types';
import { fetchWrapper } from '@/lib/fetchWrapper';
import { FieldValue, FieldValues } from 'react-hook-form';
import { revalidatePath } from 'next/cache';

/**NOTE: these functions are all on the server. */
export async function getData(query: string): Promise<PagedResults<Auction>> {
  return await fetchWrapper.get(`search${query}`);
}

export async function updateAuctionTest() {
  const data = {
    // a random number from 1 -> 100,000
    milage: Math.floor(Math.random() * 100000) + 1,
  };

  return fetchWrapper.put(
    `auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c`,
    data
  );
}

export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post('auctions', data);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}

export async function updateAuction(id: string, data: FieldValues) {
  const res = await fetchWrapper.put(`auctions/${id}`, data);

  // force the auction's cache to be cleared.
  revalidatePath(`/auctions/${id}`);

  return res;
}
