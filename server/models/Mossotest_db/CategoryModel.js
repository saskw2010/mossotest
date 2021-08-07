import CategoryModelGenerated from "./generated/CategoryModelGenerated";

const customModel = {
  
  /**
   * Customize here your schema with custom attributes
   * 
   * EXAMPLE:
    
    init() {
      let schema = CategoryModelGenerated.init();
  
      schema.add({
        extraCustomField: Object
      });
    },
     
   */


  /**
   * Override here your custom queries
   * EXAMPLE:
   *
   
    async get(id) {
      console.log("This is my custom query");
      return await CategoryModelGenerated.getModel().findOne({ _id: id });
    }

   */

};

export default {
  ...CategoryModelGenerated,
  ...customModel
};
