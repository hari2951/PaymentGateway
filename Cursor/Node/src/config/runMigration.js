const { Pool } = require('pg');
const fs = require('fs');
const path = require('path');
require('dotenv').config();

const pool = new Pool({
    user: process.env.DB_USER,
    host: process.env.DB_HOST,
    database: process.env.DB_NAME,
    password: process.env.DB_PASSWORD,
    port: process.env.DB_PORT,
});

// Read the migration file
const migrationSQL = fs.readFileSync(
    path.join(__dirname, 'migrations.sql'),
    'utf8'
);

// Run the migration
pool.query(migrationSQL, (err, res) => {
    if (err) {
        console.error('Migration failed:', err);
        return;
    }
    console.log('Migration completed successfully!');
    pool.end();
}); 