export interface Product {
    id: string;
    productName: string;
    quantity: number;
    price: number;
    promotionPrice: number;
    image: string;
}

export interface CartItem {
    product: Product;
    quantity: number;
}