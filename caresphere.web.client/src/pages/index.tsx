import Navbar from '../components/Navbar';
import Hero from '../components/Hero';
import Footer from '../components/Footer';

const Home: React.FC = () => {
  return (
    <>
    <Navbar />
    <Hero />
    <section id="features" className="py-5">
        <div className="container">
            <h2 className="text-center">Features</h2>
            <div className="row">
                <div className="col-md-4">
                    <h3>Feature One</h3>
                    <p>Some amazing feature description.</p>
                </div>
                <div className="col-md-4">
                    <h3>Feature Two</h3>
                    <p>Some amazing feature description.</p>
                </div>
                <div className="col-md-4">
                    <h3>Feature Three</h3>
                    <p>Some amazing feature description.</p>
                </div>
            </div>
        </div>
    </section>
    <Footer />
</>
  );
};

export default Home;