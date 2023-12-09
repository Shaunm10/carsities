'use server'
import { Auction, PagedResults } from "@/types";

export async function getData(pageNumber: number = 1): Promise<PagedResults<Auction>> {
    const res = await fetch(`http://localhost:6001/search?pageSize=4&pageNumber=${pageNumber}`);

    if (!res.ok) {
        throw new Error('Unable to get data');
    }

    const json = await res.json();

    return json;
}