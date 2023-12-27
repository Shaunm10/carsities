import { create } from 'zustand';

/**
 * The type of the state that is stored in our Store
 */
type State = {
  pageNumber: number;
  pageSize: number;
  pageCount: number;
  orderBy: string;
  searchTerm: string;
  searchValue: string;
};

/**
 * Functions that can be called
 * Sorta like reducers
 */
type Actions = {
  setParams: (params: Partial<State>) => void;
  reset: () => void;
  setSearchValue: (value: string) => void;
};

/**
 * The default data that's in the store
 */
const initialState: State = {
  pageNumber: 1,
  pageSize: 12,
  pageCount: 1,
  orderBy: 'make',
  searchTerm: '',
  searchValue: ''
};

/**
 * The export that contains the `Reducer` functionality
 */
export const useParamsStore = create<State & Actions>()((set) => ({
  ...initialState,

  setParams: (newParams: Partial<State>) => {
    set((state) => {
      // NOTE: this assumes will will NEVER have a pageNumber of 0
      if (newParams.pageNumber) {
        return {
          ...state,
          pageNumber: newParams.pageNumber,
        };
      } else {
        // go back to the first page.
        return {
          ...state,
          ...newParams,
          pageNumber: 1,
        };
      }
    });
  },
  reset: () => set(initialState),
  setSearchValue: (value: string) => set({ searchValue: value })
}));
