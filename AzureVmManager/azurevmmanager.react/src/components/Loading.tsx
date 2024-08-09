import { AiOutlineLoading3Quarters } from "react-icons/ai"
import "./Loading.css"

export default function Loading() {
    return (
        <div className="loading-container">
            <AiOutlineLoading3Quarters className="loading-indicator"/> Loading...
        </div>
    );
}