import { render } from "@testing-library/react";
import AccountNav from "../../components/AccountNav";
import { BrowserRouter } from "react-router-dom";

test('AccountNav component has menu items', () => {
    const { getByText } = render(<AccountNav />, { wrapper: BrowserRouter });

    const subscriptions = getByText("Subscriptions");
    expect(subscriptions).toBeInTheDocument();
    expect(subscriptions).toHaveClass("account-menu-item");
});