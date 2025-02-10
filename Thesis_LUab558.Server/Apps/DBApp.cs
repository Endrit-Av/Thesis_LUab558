using Npgsql;

namespace Thesis_LUab558.Server.Apps
{
    public class DBApp
    {
        public static void InitializeDatabase()
        {
            string databaseName = "LUab558_Thesis";
            string userName = "postgres";
            string password = "180499";
            string masterConnectionString = $"Host=localhost;Username={userName};Password={password};Database=postgres";
            string dbConnectionString = $"Host=localhost;Username={userName};Password={password};Database={databaseName}";
            string schemaName = "633867";

            try
            {
                using (var connection = new NpgsqlConnection(masterConnectionString))
                {
                    connection.Open();

                    Console.WriteLine($"Datenbank '{databaseName}' löschen, falls sie existiert...");
                    string dropDbQuery = $@"DROP DATABASE IF EXISTS ""{databaseName}"";";
                    using (var dropDbCommand = new NpgsqlCommand(dropDbQuery, connection))
                    {
                        dropDbCommand.ExecuteNonQuery();
                        Console.WriteLine($"Datenbank '{databaseName}' wurde gelöscht (falls vorhanden).");
                    }

                    Console.WriteLine($"Erstelle Datenbank '{databaseName}'...");
                    string createDbQuery = $@"CREATE DATABASE ""{databaseName}"";";
                    using (var createDbCommand = new NpgsqlCommand(createDbQuery, connection))
                    {
                        createDbCommand.ExecuteNonQuery();
                        Console.WriteLine($"Datenbank '{databaseName}' wurde erfolgreich erstellt.");
                    }
                }


                using (var dbConnection = new NpgsqlConnection(dbConnectionString))
                {
                    dbConnection.Open();

                    // Schema erstellen
                    Console.WriteLine($"Erstelle Schema '{schemaName}'...");
                    string createSchemaQuery = $@"CREATE SCHEMA IF NOT EXISTS ""{schemaName}"";";
                    ExecuteNonQuery(dbConnection, createSchemaQuery, $"Schema '{schemaName}' erstellt.");

                    // Tabellen löschen
                    Console.WriteLine("Lösche Tabellen...");
                    DeleteTables(dbConnection, schemaName);

                    // Tabellen erstellen (falls nicht vorhanden)
                    Console.WriteLine("Erstelle Tabellen...");
                    CreateTables(dbConnection, schemaName);

                    // Daten in die Tabelle 'users' einfügen
                    Console.WriteLine("Füge Benutzer in die Tabelle 'users' ein...");
                    InsertUsers(dbConnection, schemaName);

                    // Daten in die Tabelle 'products' einfügen
                    Console.WriteLine("Füge Produkte in die Tabelle 'products' ein...");
                    InsertProducts(dbConnection, schemaName);

                    // Daten in die Tabelle 'reviews' einfügen
                    Console.WriteLine("Füge Bewertungen in die Tabelle 'reviews' ein...");
                    InsertRatings(dbConnection, schemaName);

                    // Bilder in die Tabelle 'images' einfügen
                    Console.WriteLine("Füge Bilder ind die Tabelle 'images' ein...");
                    InsertProductImages(dbConnection, schemaName);
                }

                Console.WriteLine("Alle Tabellen wurden erfolgreich erstellt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        static void DeleteTables(NpgsqlConnection connection, string schemaName)
        {
            string[] dropTableQueries = {
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""invoice_items"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""invoice_details"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""wishlist"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""cart"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""reviews"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""images"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""products"";",
                        $@"DROP TABLE IF EXISTS ""{schemaName}"".""users"";"
                    };
            foreach (var query in dropTableQueries)
            {
                ExecuteNonQuery(connection, query, "Tabelle gelöscht.");
            }
        }

        static void CreateTables(NpgsqlConnection connection, string schemaName)
        {
            // 1. Tabelle: users
            string createUsersTable = $@"
                    CREATE TABLE ""{schemaName}"".""users"" (
                        user_id SERIAL PRIMARY KEY,
                        first_name VARCHAR(50),
                        last_name VARCHAR(50),
                        street VARCHAR(100),
                        house_number VARCHAR(10),
                        city VARCHAR(50),
                        postcode INTEGER,
                        date_of_birth VARCHAR(20) NOT NULL,
                        email VARCHAR(100) UNIQUE NOT NULL,
                        password VARCHAR(100),
                        telephone VARCHAR(20),
                        is_employee BOOLEAN NOT NULL DEFAULT false,
                        account_created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createUsersTable, "Tabelle 'users' erstellt.");

            // 2. Tabelle: products
            string createProductsTable = $@"
                    CREATE TABLE ""{schemaName}"".""products"" (
                        product_id SERIAL PRIMARY KEY,
                        brand VARCHAR(100) NOT NULL,
                        category VARCHAR(50) NOT NULL,
                        product_name VARCHAR(100) NOT NULL,
                        price DOUBLE PRECISION NOT NULL,
                        physical_memory INT,
                        ram INT,
                        color VARCHAR(16) NOT NULL,
                        stock INT NOT NULL,
                        description TEXT,
                        operating_system VARCHAR(30),
                        general_keyword VARCHAR(30) NOT NULL,
                        added_to_database_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createProductsTable, "Tabelle 'products' erstellt.");

            // 3. Tabelle: wishlist
            string createWishlistTable = $@"
                    CREATE TABLE ""{schemaName}"".""wishlist"" (
                        wishlist_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES ""{schemaName}"".""users""(user_id),
                        product_id INT REFERENCES ""{schemaName}"".""products""(product_id),
                        added_to_wishlist_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createWishlistTable, "Tabelle 'wishlist' erstellt.");

            // 4. Tabelle: reviews
            string createReviewsTable = $@"
                    CREATE TABLE ""{schemaName}"".""reviews"" (
                        review_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES ""{schemaName}"".""users""(user_id),
                        product_id INT REFERENCES ""{schemaName}"".""products""(product_id),
                        rating INT CHECK (rating BETWEEN 1 AND 5),
                        review_text TEXT,
                        review_date VARCHAR(20) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createReviewsTable, "Tabelle 'reviews' erstellt.");

            // 5. Tabelle: cart
            string createCartTable = $@"
                    CREATE TABLE ""{schemaName}"".""cart"" (
                        cart_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES ""{schemaName}"".""users""(user_id),
                        product_id INT REFERENCES ""{schemaName}"".""products""(product_id),
                        quantity INT DEFAULT 1,
                        added_to_cart_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createCartTable, "Tabelle 'cart' erstellt.");

            // 6. Tabelle: images
            string createImagesTable = $@"
                    CREATE TABLE ""{schemaName}"".""images"" (
                        image_id SERIAL PRIMARY KEY,
                        product_id INT REFERENCES ""{schemaName}"".""products""(product_id),
                        image_byte BYTEA NOT NULL
                    );";
            ExecuteNonQuery(connection, createImagesTable, "Tabelle 'images' erstellt.");

            // 7. Tabelle: invoice_details
            string createInvoiceDetailsTable = $@"
                    CREATE TABLE ""{schemaName}"".""invoice_details"" (
                        invoice_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES ""{schemaName}"".""users""(user_id),
                        invoice_date VARCHAR(20) NOT NULL,
                        total_amount NUMERIC(10, 2) NOT NULL,
                        payment_method VARCHAR(50) NOT NULL,
                        payment_status VARCHAR(20) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createInvoiceDetailsTable, "Tabelle 'invoice_details' erstellt.");

            // 8. Tabelle: invoice_items
            string createInvoiceItemsTable = $@"
                    CREATE TABLE ""{schemaName}"".""invoice_items"" (
                        item_id SERIAL PRIMARY KEY,
                        invoice_id INT REFERENCES ""{schemaName}"".""invoice_details""(invoice_id),
                        product_id INT REFERENCES ""{schemaName}"".""products""(product_id),
                        quantity INT NOT NULL,
                        price_per_unit NUMERIC(10, 2) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );";
            ExecuteNonQuery(connection, createInvoiceItemsTable, "Tabelle 'invoice_items' erstellt.");
        }

        static void InsertUsers(NpgsqlConnection connection, string schemaName)
        {
            var firstNames = new List<string> { "Florian", "Endrit", "Florian", "Vanessa", "Max", "Anna", "Michael", "Sophie", "Patrick", "Laura", "David", "Lisa", "Paul" };
            var lastNames = new List<string> { "Straßner", "Avdulli", "Neumann", "Müller", "Schmidt", "Fischer", "Meyers", "Star", "Star", "Hoffmann", "Meyer", "Koch", "Wasser" };
            var streets = new List<string> { "Im Ring", "Im Ring", "Im Ring", "Mutterstraße", "Hauptstraße", "Seestraße", "Waldweg", "Rosenweg", "An der Blies", "Ernst-Boehe-Straße", "Straßenstraße", "Straußenstraße", "Teststraße" };
            var houseNumbers = new List<int> { 47, 45, 45, 45, 21, 7, 12, 34, 9, 27, 14, 5, 19 };
            var cities = new List<string> { "Germersheim", "Germersheim", "Germersheim", "Mutterstadt", "Mutterstadt", "Musterstadt", "Waldstadt", "Blumenstadt", "Flussstadt", "Sonnstadt", "Baumstadt", "Waldstadt", "Feldstadt" };
            var postcodes = new List<int> { 76087, 76087, 76087, 76087, 12345, 76087, 98765, 45678, 56789, 98765, 34567, 23456, 76543 };
            var datesOfBirth = new List<string> { "1998-12-03", "1998-12-03", "1998-12-03", "1976-11-03", "1985-07-15", "1990-04-25", "1982-09-10", "1994-02-18", "1988-06-22", "1997-11-30", "1991-03-05", "1996-08-12", "1989-12-28" };
            var emails = new List<string> { "straßner@test.de", "endrit.avdulli@studmail.hwg-lu.de", "neumann_m@hotmail.de", "vanessa.mueller@notexisting.de", "max.schmidt@notexisting.com", "anna.fischer@notexisting.com", "michael.meyers@notexisting.com", "sophie.star@notexisting.com", "patrick.star@notexisting.com", "laura.hoffmann@notexisting.com", "david.meyer@notexisting.com", "lisa.koch@notexisting.com", "paul.wasser@notexisting.com" };
            var telephones = new List<string> { "062188888", "062188888", "062188888", "062188888", "0123456789", "0987654321", "0765432109", "0345678901", "0456789012", "0897612345", "0567890123", "0432167890", "0765432109" };
            var isEmployees = new List<bool> { true, true, true, false, false, false, false, false, false, false, false, false, false };

            for (int i = 0; i < firstNames.Count; i++)
            {
                string insertUserQuery = $@"
                INSERT INTO ""{schemaName}"".""users"" (first_name, last_name, street, house_number, city, postcode, date_of_birth, email, password, telephone, is_employee)
                VALUES (@first_name, @last_name, @street, @house_number, @city, @postcode, @date_of_birth, @email, @password, @telephone, @is_employee);";

                using (var command = new NpgsqlCommand(insertUserQuery, connection))
                {
                    command.Parameters.AddWithValue("first_name", firstNames[i]);
                    command.Parameters.AddWithValue("last_name", lastNames[i]);
                    command.Parameters.AddWithValue("street", streets[i]);
                    command.Parameters.AddWithValue("house_number", houseNumbers[i]);
                    command.Parameters.AddWithValue("city", cities[i]);
                    command.Parameters.AddWithValue("postcode", postcodes[i]);
                    command.Parameters.AddWithValue("date_of_birth", datesOfBirth[i]);
                    command.Parameters.AddWithValue("email", emails[i]);
                    command.Parameters.AddWithValue("password", $"{firstNames[i]}12!");
                    command.Parameters.AddWithValue("telephone", telephones[i]);
                    command.Parameters.AddWithValue("is_employee", isEmployees[i]);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"Benutzer {firstNames[i]} {lastNames[i]} erfolgreich eingefügt.");
                }
            }
        }

        public static void InsertProducts(NpgsqlConnection connection, string schemaName)
        {
            Console.WriteLine("Füge Produkte in die Tabelle 'products' ein...");
            InsertAppleProducts(connection, schemaName);
            InsertSamsungProducts(connection, schemaName);
            InsertHuaweiProducts(connection, schemaName);
            Console.WriteLine("Alle Produkte wurden erfolgreich eingefügt.");
        }

        private static void InsertAppleProducts(NpgsqlConnection connection, string schemaName)
        {
            InsertiPhone14Pro(connection, schemaName);
            InsertiPhone14ProMax(connection, schemaName);
            InsertAirPodsPro(connection, schemaName);
            InsertMacBookPro(connection, schemaName);
            InsertMacBookAir(connection, schemaName);
            InsertiPad(connection, schemaName);
            InsertiPadAir(connection, schemaName);
            Console.WriteLine("Apple-Produkte erfolgreich eingefügt.");
        }

        private static void InsertSamsungProducts(NpgsqlConnection connection, string schemaName)
        {
            InsertGalaxyZFold5(connection, schemaName);
            InsertGalaxyTabA8(connection, schemaName);
            InsertGalaxyBook3(connection, schemaName);
            Console.WriteLine("Samsung-Produkte erfolgreich eingefügt.");
        }

        private static void InsertHuaweiProducts(NpgsqlConnection connection, string schemaName)
        {
            InsertHuaweiMate50Pro(connection, schemaName);
            InsertHuaweiP60Pro(connection, schemaName);
            InsertHuaweiMateBook16s(connection, schemaName);
            InsertHuaweiMatePad(connection, schemaName);
            Console.WriteLine("Huawei-Produkte erfolgreich eingefügt.");
        }

        // Apple-Produkte
        private static void InsertiPhone14Pro(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 128, 256 };
            string[] colors = { "Weiß", "Schwarz", "Lila" };
            string description = "Das iPhone 14 überzeugt mit dem leistungsstarken A16 Bionic Chip, der für reibungslose Performance sorgt. Das 6,1-Zoll Super Retina XDR Display bietet beeindruckende Bildqualität. Kabelloses Laden bis zu 15 W und schnelles Aufladen sind praktische Funktionen. Face ID, LiDAR Scanner und Barometer ergänzen die Ausstattung des iPhone 14 für ein rundum beeindruckendes Smartphone-Erlebnis.";
            foreach (int currentMemory in memory)
            {
                double basePrice = currentMemory == 128 ? 699.99 : 899.99;
                foreach (string currentColor in colors)
                {
                    double price = basePrice + (currentColor == "Weiß" ? 250 : currentColor == "Schwarz" ? 400 : 100);
                    InsertProduct(connection, schemaName, "Apple", "Smartphone", "iPhone 14 Pro", price, currentMemory, 6, currentColor, 8, description, "iOS", "iPhone");
                }
            }
        }

        private static void InsertiPhone14ProMax(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 128, 256 };
            string[] colors = { "Weiß", "Schwarz", "Lila" };
            string description = "Das iPhone 14 Pro Max beeindruckt mit einem kraftvollen A16 Bionic Chip, der mühelos anspruchsvolle Aufgaben bewältigt. Das 6,7-Zoll Super Retina XDR Display bietet atemberaubende visuelle Erfahrungen, während die Batterie mit kabellosem Laden bis zu 15 W und schnellem Aufladen punktet. Mit Face ID, LiDAR Scanner und Barometer ist das iPhone 14 Pro Max ein technologisches Meisterwerk.";
            foreach (int currentMemory in memory)
            {
                double basePrice = currentMemory == 128 ? 1099.99 : 1499.99;
                foreach (string currentColor in colors)
                {
                    InsertProduct(connection, schemaName, "Apple", "Smartphone", "iPhone 14 Pro Max", basePrice, currentMemory, 6, currentColor, 12, description, "iOS", "iPhone");
                }
            }
        }

        private static void InsertAirPodsPro(NpgsqlConnection connection, string schemaName)
        {
            string description = "Die geringe Verzerrung und die speziell entwickelten Treiber und Verstärker liefern brillante Höhen und Tiefe, satte Bässe in beeindruckender Klarheit. Bis zu 30 Std. Wiedergabe insgesamt mit eingeschalteter Aktiver Geräuschunterdrückung und dem MagSafe Ladecase – 6 Std. mehr als bei den AirPods Pro (1. Generation). Durch die In-Ear-Erkennung und das automatische Wechseln zwischen Geräten funktioniert alles nahtlos.";
            InsertProduct(connection, schemaName, "Apple", "Kopfhörer", "AirPods Pro 2. Generation", 249.99, null, null, "Weiß", 23, description, null, "airpods");
        }

        private static void InsertMacBookPro(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 256, 512 };
            int[] ram = { 8, 16 };
            string description = "Das Apple MacBook Pro der neuesten Generation bietet erstklassige Leistung mit dem Apple M2 Chip und eine großzügige SSD für blitzschnelle Datenzugriffe. Das atemberaubende Retina Display mit hohem Kontrast und lebendigen Farben sorgt für ein herausragendes visuelles Erlebnis, während die lange Akkulaufzeit und das schlanke, leichte Design es zum perfekten Begleiter für unterwegs machen.";
            foreach (int currentMemory in memory)
            {
                foreach (int currentRam in ram)
                {
                    double basePrice = currentMemory == 256 ? 1899.99 : 2199.99;
                    basePrice += currentRam == 8 ? 200 : 300;
                    InsertProduct(connection, schemaName, "Apple", "Notebook", "MacBook Pro", basePrice, currentMemory, currentRam, "Silber", 238, description, "macOS", "MacBook");
                }
            }
        }

        private static void InsertMacBookAir(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 256, 512 };
            int[] ram = { 8, 16 };
            string description = "Das Apple MacBook Air mit dem revolutionären Apple M1 Chip bietet herausragende Leistung, eine SSD und einen leistungsstarken CPU, ideal für unterwegs. Mit seinem schlanken und leichten Design, dem beeindruckenden Retina Display und der langen Akkulaufzeit ist es die perfekte Wahl für produktives Arbeiten und Entertainment unterwegs.";
            foreach (int currentMemory in memory)
            {
                foreach (int currentRam in ram)
                {
                    double basePrice = currentMemory == 256 ? 899.00 : 999.00;
                    basePrice += currentRam == 8 ? 200 : 300;
                    InsertProduct(connection, schemaName, "Apple", "Notebook", "MacBook Air M1", basePrice, currentMemory, currentRam, "Silber", 10, description, "macOS", "MacBook");
                }
            }
        }

        private static void InsertiPad(NpgsqlConnection connection, string schemaName)
        {
            string[] colors = { "Grau", "Silber" };
            string description = "Das Apple iPad 10.2 Wi-Fi der 9ten Generation ist ein vielseitiges und leistungsstarkes Tablet. Es verfügt über ein beeindruckendes 10,2-Zoll Retina-Display, einen schnellen A13 Bionic Chip und läuft auf iPadOS 15. Mit 64 GB internem Speicher und einer Akkulaufzeit von bis zu 10 Stunden ist es ideal für Arbeit und Unterhaltung.";
            foreach (string currentColor in colors)
            {
                InsertProduct(connection, schemaName, "Apple", "Tablet", "iPad 10.2", 679.99, 64, 3, currentColor, 10, description, "iPadOS", "iPad");
            }
        }

        private static void InsertiPadAir(NpgsqlConnection connection, string schemaName)
        {
            string[] colors = { "Blau", "Lila", "Pink" };
            string description = "Das Apple iPad Air hebt deine Erlebnisse auf ein neues Level, egal ob du liest, Videos anschaust oder kreativ arbeitest. Mit seinem beeindruckenden 10,9-Zoll Liquid Retina Display, das fortschrittliche Technologien wie True Tone, einen großzügigen P3 Farbraum und eine Antireflex-Beschichtung bietet, tauchst du in eine Welt gestochen scharfer Farben und Details ein.";
            foreach (string currentColor in colors)
            {
                InsertProduct(connection, schemaName, "Apple", "Tablet", "iPad Air", 599.99, 64, 8, currentColor, 10, description, "iPadOS", "iPad");
            }
        }

        // Samsung-Produkte
        private static void InsertGalaxyZFold5(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 128, 256 };
            string[] colors = { "Schwarz", "Blau" };
            string description = "Das Samsung Galaxy Z Fold5 ist ein leistungsstarkes Smartphone mit einem beeindruckenden 7,6-Zoll Dynamic AMOLED Touchscreen Display. Es verfügt über eine 50 MP + 12 MP + 10 MP Kamera mit Autofokus und 10-fach Zoom oder Fotolicht. Das Betriebssystem ist Android 13 in Kombination mit One UI 5.1 und KNOX 3.9.";
            foreach (int currentMemory in memory)
            {
                foreach (string currentColor in colors)
                {
                    double price = currentMemory == 128 ? 999.99 : 1299.99;
                    InsertProduct(connection, schemaName, "Samsung", "Smartphone", "Galaxy Z Fold5", price, currentMemory, 12, currentColor, 0, description, "Android 13", "Galaxy");
                }
            }
        }

        private static void InsertGalaxyTabA8(NpgsqlConnection connection, string schemaName)
        {
            string[] colors = { "Grau", "Silber" };
            string description = "Das Samsung Galaxy Tab A8 Wi-Fi ist ein beeindruckendes 10,5-Zoll Tablet mit einem schlanken Design. Es bietet großartige Unterhaltungsmöglichkeiten, darunter Surround-Sound aus vier Lautsprechern, Apps und Spiele für die ganze Familie sowie Zugang zu Samsung TV Plus für vielfältige kostenlose Inhalte.";
            foreach (string currentColor in colors)
            {
                InsertProduct(connection, schemaName, "Samsung", "Tablet", "Galaxy Tab A8", 159.99, 32, 3, currentColor, 112, description, "Android", "Galaxy");
            }
        }

        private static void InsertGalaxyBook3(NpgsqlConnection connection, string schemaName)
        {
            string description = "Das Galaxy Book3 von Samsung ist ein 15,6-Zoll Notebook mit Full HD IPS-Display. Es wird von einem Intel® Core™ i5-1335U Prozessor mit bis zu 3,4 GHz, 8 GB LPDDR4x RAM und einer 512 GB SSD angetrieben. Vorinstalliert ist Windows® 11 Home. Mit USB- und HDMI-Schnittstellen für Peripheriegeräte, Frontkamera für Videoanrufe und einer beleuchteten Tastatur ist es ideal für unterwegs.";
            InsertProduct(connection, schemaName, "Samsung", "Notebook", "Galaxy Book3", 999.99, 256, 16, "Grau", 71, description, "Windows 11", "Galaxy");
        }

        // Huawei-Produkte
        private static void InsertHuaweiMate50Pro(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 256, 512 };
            string[] colors = { "Schwarz", "Silber" };
            string description = "Das Huawei Mate 50 Pro ist ein hochwertiges Smartphone mit beeindruckender Leistung und vielen Funktionen. Es verfügt über einen 17,12 cm großen Bildschirm (2616 x 1212 Pixel), einen Qualcomm Snapdragon 8+ Gen 1 Octa-Core-Prozessor, 8 GB RAM und bietet wahlweise 256 GB oder 512 GB internen Speicher.";
            foreach (int currentMemory in memory)
            {
                foreach (string currentColor in colors)
                {
                    double price = currentMemory == 256 ? 799.00 : 849.99;
                    InsertProduct(connection, schemaName, "Huawei", "Smartphone", "Mate 50 Pro", price, currentMemory, 8, currentColor, 200, description, "Huawei OS", "Mate");
                }
            }
        }

        private static void InsertHuaweiP60Pro(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 128, 256, 512 };
            string[] colors = { "Schwarz", "Weiß" };
            string description = "Das Huawei P60 Pro ist ein leistungsstarkes Smartphone mit einem 16,9 cm Full HD-Bildschirm (2700 x 1220 Pixel). Es wird von einem Qualcomm Snapdragon 8+ Gen 1 Octa-Core-Prozessor mit 3,2 GHz, 8 GB RAM und einer großen Speicherkapazität angetrieben. Die Kameras haben 48 MP (Hauptkamera), 13 MP (zweite Hauptkamera) und 13 MP (Frontkamera).";
            foreach (int currentMemory in memory)
            {
                foreach (string currentColor in colors)
                {
                    double price = currentMemory == 128 ? 899.00 : currentMemory == 256 ? 999.00 : 1150.00;
                    InsertProduct(connection, schemaName, "Huawei", "Smartphone", "P60 Pro", price, currentMemory, 8, currentColor, 200, description, "EMUI 13.1", "P60");
                }
            }
        }

        private static void InsertHuaweiMateBook16s(NpgsqlConnection connection, string schemaName)
        {
            string description = "Das Huawei MateBook 16s Notebook ist ein leistungsstarkes Laptop mit beeindruckenden technischen Spezifikationen. Es verfügt über einen 16-Zoll Bildschirm mit 2.5K-Auflösung und IPS-Display-Technologie. Die hohe Bildschirmhelligkeit und Farbtiefe bieten ein hervorragendes visuelles Erlebnis.";
            InsertProduct(connection, schemaName, "Huawei", "Notebook", "MateBook 16s", 1899.00, 1000, 16, "Silber", 152, description, "Windows 11 Home", "MateBook");
        }

        private static void InsertHuaweiMatePad(NpgsqlConnection connection, string schemaName)
        {
            int[] memory = { 128, 256, 512 };
            string description = "Das Huawei MatePad SE WiFi 4 Tablet ist ein leistungsstarkes Tablet mit beeindruckenden technischen Spezifikationen. Es verfügt über einen 10,4-Zoll großen Bildschirm mit einer Auflösung von 2000 x 1200 Pixeln und verwendet IPS-Technologie. Angetrieben wird es von einem Octa-Core Qualcomm Snapdragon 680 Prozessor.";
            foreach (int currentMemory in memory)
            {
                double price = currentMemory == 128 ? 249.00 : currentMemory == 256 ? 300.00 : 349.99;
                InsertProduct(connection, schemaName, "Huawei", "Tablet", "MatePad SE WiFi 4", price, currentMemory, 4, "Schwarz", 83, description, "HarmonyOS", "MatePad");
            }
        }

        // Hilfsmethode für das Einfügen von Produkten
        private static void InsertProduct(NpgsqlConnection connection, string schemaName, string brand, string category, string productName, double price, int? physicalMemory, int? ram, string color, int stock, string description, string operatingSystem, string generalKeyword)
        {
            string query = $@"
            INSERT INTO ""{schemaName}"".""products"" 
            (brand, category, product_name, price, physical_memory, ram, color, stock, description, operating_system, general_keyword) 
            VALUES (@brand, @category, @product_name, @price, @physical_memory, @ram, @color, @stock, @description, @operating_system, @general_keyword);";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("brand", brand);
                command.Parameters.AddWithValue("category", category);
                command.Parameters.AddWithValue("product_name", productName);
                command.Parameters.AddWithValue("price", price);
                command.Parameters.AddWithValue("physical_memory", (object)physicalMemory ?? DBNull.Value);
                command.Parameters.AddWithValue("ram", (object)ram ?? DBNull.Value);
                command.Parameters.AddWithValue("color", color);
                command.Parameters.AddWithValue("stock", stock);
                command.Parameters.AddWithValue("description", description);
                command.Parameters.AddWithValue("operating_system", (object)operatingSystem ?? DBNull.Value);
                command.Parameters.AddWithValue("general_keyword", generalKeyword);
                command.ExecuteNonQuery();
            }
        }

        public static void InsertRatings(NpgsqlConnection connection, string schemaName)
        {
            Console.WriteLine("Füge Bewertungen in die Tabelle 'reviews' ein...");

            // Benutzer- und Produktlisten
            List<int> allUsers = GetAllUsers(connection, schemaName);
            List<int> allProducts = GetAllProducts(connection, schemaName);

            // Ratings einfügen
            InsertIntoRatings(connection, schemaName, allUsers, allProducts);

            Console.WriteLine("Die Reviews wurden erfolgreich hinzugefügt.");
        }

        private static List<int> GetAllUsers(NpgsqlConnection connection, string schemaName)
        {
            List<int> allUsers = new List<int>();
            string query = $@"SELECT user_id FROM ""{schemaName}"".""users"";";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allUsers.Add(reader.GetInt32(0));
                    }
                }
            }

            Console.WriteLine($"Gefundene Benutzer: {allUsers.Count}");
            return allUsers;
        }

        private static List<int> GetAllProducts(NpgsqlConnection connection, string schemaName)
        {
            List<int> allProducts = new List<int>();
            string query = $@"SELECT product_id FROM ""{schemaName}"".""products"";";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allProducts.Add(reader.GetInt32(0));
                    }
                }
            }

            Console.WriteLine($"Gefundene Produkte: {allProducts.Count}");
            return allProducts;
        }

        private static void InsertIntoRatings(NpgsqlConnection connection, string schemaName, List<int> allUsers, List<int> allProducts)
        {
            Random random = new Random();
            string query = $@"
            INSERT INTO ""{schemaName}"".""reviews"" 
            (user_id, product_id, rating, review_text, review_date) 
            VALUES (@user_id, @product_id, @rating, @review_text, @review_date);";

            foreach (int userId in allUsers)
            {
                foreach (int productId in allProducts)
                {
                    if (productId != 1) // Produkt ausschließen
                    {
                        int rating = random.Next(1, 6); // Zufällige Bewertung zwischen 1 und 5
                        string reviewText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.";
                        string reviewDate = DateTime.Now.ToString("yyyy-MM-dd");

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("user_id", userId);
                            command.Parameters.AddWithValue("product_id", productId);
                            command.Parameters.AddWithValue("rating", rating);
                            command.Parameters.AddWithValue("review_text", reviewText);
                            command.Parameters.AddWithValue("review_date", reviewDate);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void InsertProductImages(NpgsqlConnection connection, string schemaName)
        {
            Console.WriteLine("Füge Bilder zu Produkten in die Tabelle 'images' ein...");

            // LinkedHashMap von Java wird durch Dictionary ersetzt
            Dictionary<string, int> allPaths = ReadAllProductsFromDb(connection, schemaName);

            string query = $@"
            INSERT INTO ""{schemaName}"".""images"" (product_id, image_byte) 
            VALUES (@product_id, @image_byte);";

            foreach (var path in allPaths.Keys)
            {
                try
                {
                    byte[] imageBytes = GetImageBytes(path);
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("product_id", allPaths[path]);
                        command.Parameters.AddWithValue("image_byte", imageBytes);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Bild für Produkt-ID {allPaths[path]} erfolgreich hinzugefügt.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Hinzufügen des Bildes für Produkt-ID {allPaths[path]}: {ex.Message}");
                }
            }

            Console.WriteLine("Alle Bilder wurden erfolgreich hinzugefügt.");
        }

        private static Dictionary<string, int> ReadAllProductsFromDb(NpgsqlConnection connection, string schemaName)
        {
            Dictionary<string, int> allPaths = new Dictionary<string, int>();
            string query = $@"SELECT brand, product_name, color, product_id FROM ""{schemaName}"".""products"";";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string brand = reader.GetString(0);
                        string productName = reader.GetString(1);
                        string color = reader.GetString(2);
                        int productId = reader.GetInt32(3);
                        FillAllPaths(allPaths, brand, productName, color, productId);
                    }
                }
            }

            return allPaths;
        }

        private static void FillAllPaths(Dictionary<string, int> allPaths, string brand, string productName, string color, int productId)
        {
            // Passt den Produktnamen für den Dateipfad an
            string adjustedProductName = AdjustProductName(brand, productName);

            int quantity = CheckImageQuantity(brand, adjustedProductName, color);
            int index = 1;

            // Bilder für "AirPods Pro 2. Generation" auslassen
            if (!productName.Equals("AirPods Pro 2. Generation", StringComparison.OrdinalIgnoreCase))
            {
                if (quantity > 0)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Thesis_LUab558.Server", "wwwroot", "Images", "Products");
                    string relativePath = Path.Combine(basePath, brand, $"{adjustedProductName}_{color}");

                    // Main-Bild
                    string mainImagePath = $"{relativePath}_Main.jpeg";
                    allPaths[mainImagePath] = productId;

                    // Zusätzliche Bilder
                    while (quantity > 0)
                    {
                        string imagePath = $"{relativePath}_{index}.jpeg";
                        allPaths[imagePath] = productId;
                        quantity--;
                        index++;
                    }
                }
                else
                {
                    Console.WriteLine($"Keine Bilder für Produkt-ID {productId} gefunden.");
                }
            }
        }

        private static string AdjustProductName(string brand, string productName)
        {
            // Falls "Galaxy" im Produktnamen enthalten ist, entfernen
            if (brand.Equals("Samsung", StringComparison.OrdinalIgnoreCase))
            {
                return productName.Replace("Galaxy ", "");
            }

            // Für andere Marken keine Anpassung
            return productName;
        }

        private static int CheckImageQuantity(string brand, string productName, string color)
        {
            int index = 0;
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Thesis_LUab558.Server", "wwwroot", "Images", "Products");

            for (int i = 1; i <= 10; i++)
            {
                string relativePath = Path.Combine(basePath, brand, $"{productName}_{color}_{i}.jpeg");

                if (File.Exists(relativePath))
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            return index;
        }

        private static byte[] GetImageBytes(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Bilddatei nicht gefunden: {path}");
            }

            return File.ReadAllBytes(path);
        }

        static void ExecuteNonQuery(NpgsqlConnection connection, string query, string successMessage)
        {
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine(successMessage);
            }
        }
    }
}
