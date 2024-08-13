// LinkeIn Post : 
// Now I will Show U [CascadeMode.StopOnFirstFailure , CascadeMode.Stop] in Rule-Level 

// [Example 1]: Using StopOnFirstFailure in Rule-Level
// If Surname NotNull fails, then Surname NotEqual will not be run.
public class PersonValidator : AbstractValidator<Person> {
  public PersonValidator() {

    // Regardless of this, Forename rules will always be run.
    RuleFor(x => x.Surname).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEqual("foo");
    RuleFor(x => x.Forename).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEqual("foo")
  }
}

var person = new Person { Surname = null, Forename = null };
var result = new PersonValidator().Validate(person);
// This will generate 2 errors: 
// - Surname error (because it is null). Additional validation in the first chain (NotEqual) has not run
// - Forename error (because it is null). Additional validation in the first chain (NotEqual) has not run
// ========== ========= ===========
// [Example 2]: Using Stop. This will behave exactly the same as example 1:[cause it in the Rule-Level]
// If Surname NotNull fails, then Surname NotEqual will not be run.
// Regardless of this, Forename rules will always be run.
// Cascade mode is always *scoped to the level it is set*, so in this case because you set Stop in a specific call to RuleFor
// it will *only affect that rule chain*.
// If Surname NotNull fails, then Surname NotEqual will not be run.
// Regardless of this, Forename rules will always be run.
public class PersonValidator : AbstractValidator<Person> {
  public PersonValidator() {
    RuleFor(x => x.Surname).Cascade(CascadeMode.Stop).NotNull().NotEqual("foo");
    RuleFor(x => x.Forename).Cascade(CascadeMode.Stop).NotNull().NotEqual("foo");
  }
}

var person = new Person { Surname = null, Forename = null };
var result = new PersonValidator().Validate(person);
// This will generate the same 2 errors: 
// - Surname error (because it is null). Additional validation in the first chain (NotEqual) has not run
// - Forename error (because it is null). Additional validation in the first chain (NotEqual) has not run





// ============================= In Class-Level =============================== 
// هنا هنلاحظ الفرق بين الاتنين 
// [Example 3]: StopOnFirstFailure at class level. 
public class PersonValidator : AbstractValidator<Person> {
  public PersonValidator() {
     this.CascadeMode = CascadeMode.StopOnFirstFailure;
      
    // In this example, the cascade is set at the "class level", using the old StopOnFirstFailure behaviour.
    // This will behave exactly the same as the previous example. 
    // If Surname NotNull fails, then Surname NotEqual will not be run.
    // Regardless of this, Forename rules will always be run.
    RuleFor(x => x.Surname).NotNull().NotEqual("foo");
    RuleFor(x => x.Forename).NotNull().NotEqual("foo")
   }
}
var person = new Person { Surname = null, Forename = null };
var result = new PersonValidator().Validate(person);
// Behaves exactly the same as examples 1 & 2. 
// This will generate 2 errors: validate for all properties rules قولنا هيكمل باقي الرولز ويعمل 
// - Surname error (because it is null). Additional validation in the first chain (NotEqual) has not run
// - Forename error (because it is null). Additional validation in the first chain (NotEqual) has not run

// Example 4: Stop at class level. 
public class PersonValidator : AbstractValidator<Person> {
  public PersonValidator() {
     this.CascadeMode = CascadeMode.Stop;
      
    // In this example, the cascade is set at the class level, using the new Stop behaviour.
    // This is a "fail fast" behaviour. Only a maximum of 1 validation failure will ever be generated.
    
    // If Surname NotNull fails, then Surname NotEqual will not be run, Forename NotNull will not be run and Forename NotEqual will not be run. 
    // هتحصل fail عند اول  stop وه validation هنا مش هيكمل 
    RuleFor(x => x.Surname).NotNull().NotEqual("foo");
    RuleFor(x => x.Forename).NotNull().NotEqual("foo")
   }
}

var person = new Person { Surname = null, Forename = null };
var result = new PersonValidator().Validate(person);
// This will generate only 1 error: 
// - Surname error (because it is null). Validator immediately exits and nothing more is run. 



