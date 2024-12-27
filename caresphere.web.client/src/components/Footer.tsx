export default function Footer() {
    return (
        <footer className="bg-dark text-white text-center py-3">
            <div className="container">
                <p className="mb-0">&copy; {new Date().getFullYear()} MyNextApp. All rights reserved.</p>
            </div>
        </footer>
    );
}
