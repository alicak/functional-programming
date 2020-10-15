package transforming

import scala.util.{Success, Try}

object Transformer {
  val peopleStrings = People.loadPeople.drop(1) // drop the first line with headers

  def parse(personString: String): Person = {
    def stringToOption(s: String): Option[String] = s match {
      case "" => None
      case s: String => Some(s)
    }

    def parsePhone(phone: String): Option[Phone] = {
      val regex = raw"(\d{3})-(\d{4})-(\d{2})".r
      phone match {
        case regex(start, middle, end) => Some(Phone(start.toInt, middle.toInt, end.toInt))
        case _ => None
      }
    }

    def parseNumber(number: String): Option[Int] = {
      Try(number.toInt) match {
        case Success(value) => Some(value)
        case _ => None
      }
    }

    val record = personString.split(";", -1).toList
    val optionRecord = record.map(stringToOption)

    Person(
      name = optionRecord(0),
      surname = optionRecord(1),
      phone = optionRecord(2).flatMap(parsePhone),
      experience = optionRecord(3).flatMap(parseNumber)
    )
  }

  def transform(person: Person) : Person = {
    def redirectPhone(phone: Phone): Phone = {
      phone.copy(start = 123)
    }
    person.copy(phone = person.phone.map(redirectPhone))
  }

  def print(person: Person) = {
    s"(Name: ${person.name.getOrElse("n/a")}, " +
      s"Surname: ${person.surname.getOrElse("n/a")}, " +
      s"Phone: ${person.phone.getOrElse("-")}, " +
      s"Experience: ${person.experience.getOrElse(0)})"
  }

  def process() = {
    peopleStrings
      .map(parse)
      .map(transform)
      .map(print)
  }
}