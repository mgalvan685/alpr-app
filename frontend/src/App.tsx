import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Sidebar } from "./components/layout/Sidebar";
import { PageContainer } from "./components/layout/PageContainer";

import VideosPage from "./pages/VideosPage";
import VideoDetailPage from "./pages/VideoDetailPage";
import UploadPage from "./pages/UploadPage";
import DashboardPage from "./pages/DashboardPage";
import PlateDetailPage from "./pages/PlateDetailPage";
import PlatesPage from "./pages/PlatesPage";
import { Header } from "./components/layout/Header";
import ViewAllSightingsPage from "./pages/ViewAllSightingsPage";

export default App;

function App() {
  return (
    <BrowserRouter>
      <div className="flex">
        <Sidebar />
        <div className="flex-1 flex flex-col">
            <Header />
            <PageContainer>
                <Routes>
                    <Route path="/" element={<DashboardPage />} />
                    <Route path="/videos" element={<VideosPage />} />
                    <Route path="/videos/:id" element={<VideoDetailPage />} />
                    <Route path="/upload" element={<UploadPage />} />
                    <Route path="/plates" element={<PlatesPage />} />
                    <Route path="/plates/:plate" element={<PlateDetailPage />} />
                    <Route path="/sightings" element={<ViewAllSightingsPage />} />
                </Routes>
            </PageContainer>
        </div>
      </div>
    </BrowserRouter>
  );
}