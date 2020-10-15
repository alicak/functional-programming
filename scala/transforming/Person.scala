package transforming

case class Phone(start: Int, middle: Int, end: Int) {
  override def toString: String = s"${start}-${middle}-${end}"
}

case class Person(name: Option[String], surname: Option[String], phone: Option[Phone], experience: Option[Int])
