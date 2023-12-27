import { orderByValues } from "@/types/orderBy";
import { IconType } from "react-icons";
import { AiOutlineClockCircle, AiOutlineSortAscending } from "react-icons/ai";
import { BsFillStopCircleFill } from "react-icons/bs";

export const orderButtons: {
    label: string;
    icon: IconType;
    value: orderByValues;
}[] = [
        {
            label: 'Alphabetical',
            icon: AiOutlineSortAscending,
            value: 'make',
        },
        {
            label: 'End date',
            icon: AiOutlineClockCircle,
            value: 'endingSoon',
        },
        {
            label: 'Recently added',
            icon: BsFillStopCircleFill,
            value: 'new',
        },
    ];