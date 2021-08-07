// Import Mongoose
import mongoose from "mongoose";
// Logging
import Logger from "./Logger";
// Properties
import properties from "../properties.js";

// Start Import Models

import UserModel from "../models/Mossotest_db/UserModel";
import CategoryModel from "../models/Mossotest_db/CategoryModel";
import ProductModel from "../models/Mossotest_db/ProductModel";

// End Import Models

class Database {
  constructor() {}

  /**
   * Init database
   */
  async init() {
    await this.authenticate();
    Logger.info("MongoDB connected at: " + properties.mossotest_db_dbUrl);

    // Start Init Models

		UserModel.init();
		CategoryModel.init();
		ProductModel.init();
 // End Init Models
  }

  /**
   * Start database connection
   */
  async authenticate() {
    Logger.info("Authenticating to the databases...");
    try {
      this.dbConnection_mossotest_db = await mongoose.connect(
        "mongodb://" + properties.mossotest_db_dbUrl,
        { useNewUrlParser: true }
      );
    } catch (err) {
      Logger.error(`Failed connection to the DB: ${err.message}`);
      Logger.error(err);
      await new Promise(resolve => setTimeout(resolve, 5000));
      await this.authenticate();
    }
  }

  /**
   * Get connection db
   */
  getConnection() {
    return this.dbConnection_mossotest_db;
  }
}

export default new Database();
