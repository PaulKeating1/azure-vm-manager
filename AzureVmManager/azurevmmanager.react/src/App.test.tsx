import { render, waitFor } from '@testing-library/react';
import App from './App';
import { MsalReactTester } from 'msal-react-tester';

let msalTester: MsalReactTester;

beforeEach(() => {
  // new instance of msal tester for each test:
  msalTester = new MsalReactTester(); 
  // or new MsalReactTester("Redirect") / new MsalReactTester("Popup")

  // Ask msal-react-tester to handle and mock all msal-react processes:
  msalTester.spyMsal();
});

afterEach(() => {
  // reset msal-react-tester
  msalTester.resetSpyMsal();
});

jest.mock('./components/SiteHeader', () => {
  const SiteHeader = () => <div data-testid="SiteHeader">SiteHeader</div>;
  return SiteHeader;
});

test('App renders site header component', async () => {
  await msalTester.isNotLogged();
  const { getByTestId } = await waitFor(() => render(<App pca={msalTester.client} />));
  expect(getByTestId("SiteHeader")).toBeInTheDocument();
});
