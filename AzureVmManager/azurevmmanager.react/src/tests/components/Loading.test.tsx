import { render } from "@testing-library/react";
import Loading from "../../components/Loading";

test("Loading icon and text are displayed", () => {
    const { queryByText, container } = render(<Loading />,);
    expect(queryByText("Loading...")).toBeInTheDocument();
    const icon = container.querySelector("svg");
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveClass("MuiCircularProgress-svg");
});