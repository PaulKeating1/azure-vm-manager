import Home from './pages/Home';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";
import './App.css';
import SiteHeader from './components/SiteHeader';

function App() {
  return (
    <div className="App">    
      <Router>
        <SiteHeader />
        <Routes>
            <Route path="/" element={<Home />}/>
        </Routes>
      </Router>    
    </div>
  );
}

export default App;
