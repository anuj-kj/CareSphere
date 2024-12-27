# CareSphere.Web.Client

## Overview
CareSphere.Web.Client is a React Next.js application built with TypeScript. This project serves as a web client for the CareSphere application, providing a responsive and interactive user interface.

## Getting Started

### Prerequisites
- Node.js (version 12 or later)
- npm (Node Package Manager)

### Installation
1. Clone the repository:
   ```
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```
   cd CareSphere.Web.Client
   ```
3. Install the dependencies:
   ```
   npm install
   ```

### Running the Application
To start the development server, run:
```
npm run dev
```
This will start the application at `http://localhost:3000`.

### Building for Production
To build the application for production, run:
```
npm run build
```
After building, you can start the production server with:
```
npm run start
```

## Project Structure
```
CareSphere.Web.Client
├── public                # Static files
│   └── favicon.ico      # Favicon for the web application
├── src                   # Source files
│   ├── pages            # Page components
│   │   ├── _app.tsx     # Custom App component
│   │   ├── _document.tsx # Custom Document component
│   │   └── index.tsx    # Main entry point
│   ├── components        # Reusable components
│   │   └── ExampleComponent.tsx
│   ├── styles           # CSS files
│   │   ├── globals.css   # Global styles
│   │   └── Home.module.css # Scoped styles for Home component
│   └── types            # TypeScript types and interfaces
│       └── index.ts
├── .eslintrc.json       # ESLint configuration
├── .gitignore           # Git ignore file
├── next-env.d.ts       # TypeScript types for Next.js
├── next.config.js       # Next.js configuration
├── package.json         # npm configuration
├── tsconfig.json        # TypeScript configuration
└── README.md            # Project documentation
```

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.