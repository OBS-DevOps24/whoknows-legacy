const About = () => {
    return (
        <div className="max-w-2xl mx-auto text-center space-y-8 py-10">
            <div className="space-y-4">
                <h1 className="text-3xl font-bold text-gray-800">Our mission</h1>
                <p className="text-lg text-gray-600">
                    We intend to build the world's best search engine!
                </p>
            </div>

            <div className="space-y-4">
                <h2 className="text-2xl font-bold text-gray-800">Our team</h2>
                <p className="text-lg text-gray-600">
                    <span className="font-medium">Osman</span> • 
                    <span className="font-medium"> Benjamin</span> • 
                    <span className="font-medium"> Sham</span>
                </p>
            </div>
        </div>
    );
};

export default About;