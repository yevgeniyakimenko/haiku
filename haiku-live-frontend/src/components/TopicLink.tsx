import { Topic } from '../../types'

interface TopicLinkProps {
  topic: Topic
  selected: boolean
  onClick: (topic: Topic) => void
}

export default function AuthorLink({ topic, selected, onClick }: TopicLinkProps) {
  const selAuth: string = selected ? 'font-extrabold text-orange-700 hover:text-amber-600' : ''
  const handleClick: () => void = () => {
    onClick(topic)
  }

  return (
    <li className={`mb-3 cursor-pointer break-all hover:text-amber-700 hover:font-bold ${selAuth}`} onClick={handleClick}>
      {selected ? '🍂 ' : ''}
      {topic.name}
    </li>
  )
}
