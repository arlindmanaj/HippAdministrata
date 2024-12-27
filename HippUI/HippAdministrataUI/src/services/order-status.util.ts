import { OrderStatus } from "../models/OrderStatus";

export function getOrderStatusLabel(status: number | string): string {
    // If the status is already a string, return it
    if (typeof status === 'string') {
        return status;
    }

    // Convert numeric status to the corresponding string value
    return OrderStatus[status] || 'Unknown';
}